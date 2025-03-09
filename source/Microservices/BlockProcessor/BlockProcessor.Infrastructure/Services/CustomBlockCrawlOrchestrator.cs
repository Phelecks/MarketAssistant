﻿using Nethereum.BlockchainProcessing.BlockProcessing.CrawlerSteps;
using Nethereum.BlockchainProcessing.BlockProcessing;
using Nethereum.BlockchainProcessing.Orchestrator;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Nethereum.Contracts.Services;
using Nethereum.RPC.Eth.DTOs;
using System.Numerics;
using Nethereum.Contracts;
using BlockProcessor.Application.Interfaces;

namespace BlockProcessor.Infrastructure.Services
{
    public class CustomBlockCrawlOrchestrator : ICustomBlockchainProcessingOrchestrator
    {
        public IEthApiContractService EthApi { get; set; }
        public IEnumerable<BlockProcessingSteps> ProcessingStepsCollection { get; }
        public BlockCrawlerStep BlockCrawlerStep { get; }
        public TransactionCrawlerStep TransactionWithBlockCrawlerStep { get; }
        public TransactionReceiptCrawlerStep TransactionWithReceiptCrawlerStep { get; }
        public ContractCreatedCrawlerStep ContractCreatedCrawlerStep { get; }

        public FilterLogCrawlerStep FilterLogCrawlerStep { get; }

        public CustomBlockCrawlOrchestrator(IEthApiContractService ethApi, BlockProcessingSteps blockProcessingSteps)
            : this(ethApi, new[] { blockProcessingSteps })
        {

        }

        public CustomBlockCrawlOrchestrator(IEthApiContractService ethApi, IEnumerable<BlockProcessingSteps> processingStepsCollection)
        {

            this.ProcessingStepsCollection = processingStepsCollection;
            EthApi = ethApi;
            BlockCrawlerStep = new BlockCrawlerStep(ethApi);
            TransactionWithBlockCrawlerStep = new TransactionCrawlerStep(ethApi);
            TransactionWithReceiptCrawlerStep = new TransactionReceiptCrawlerStep(ethApi);
            ContractCreatedCrawlerStep = new ContractCreatedCrawlerStep(ethApi);
            FilterLogCrawlerStep = new FilterLogCrawlerStep(ethApi);
        }

        public virtual async Task CrawlBlockAsync(BigInteger blockNumber)
        {
            var blockCrawlerStepCompleted = await BlockCrawlerStep.ExecuteStepAsync(blockNumber, ProcessingStepsCollection).ConfigureAwait(false);
            await CrawlTransactionsAsync(blockCrawlerStepCompleted).ConfigureAwait(false);

        }
        protected virtual async Task CrawlTransactionsAsync(CrawlerStepCompleted<BlockWithTransactions> completedStep)
        {
            if (completedStep != null)
            {
                foreach (var txn in completedStep.StepData.Transactions)
                {
                    await CrawlTransactionAsync(completedStep, txn).ConfigureAwait(false);
                }
            }
        }
        protected virtual async Task CrawlTransactionAsync(CrawlerStepCompleted<BlockWithTransactions> completedStep, Transaction txn)
        {
            var currentStepCompleted = await TransactionWithBlockCrawlerStep.ExecuteStepAsync(
                new TransactionVO(txn, completedStep.StepData), completedStep.ExecutedStepsCollection).ConfigureAwait(false);

            if (currentStepCompleted.ExecutedStepsCollection.Any() && TransactionWithReceiptCrawlerStep.Enabled)
            {
                await CrawlTransactionReceiptAsync(currentStepCompleted).ConfigureAwait(false);
            }
        }

        protected virtual async Task CrawlTransactionReceiptAsync(CrawlerStepCompleted<TransactionVO> completedStep)
        {
            if (TransactionWithReceiptCrawlerStep.Enabled)
            {
                var currentStepCompleted = await TransactionWithReceiptCrawlerStep.ExecuteStepAsync(
                    completedStep.StepData,
                    completedStep.ExecutedStepsCollection).ConfigureAwait(false);
                if (currentStepCompleted != null && currentStepCompleted.StepData.IsForContractCreation() &&
                    ContractCreatedCrawlerStep.Enabled)
                {
                    await ContractCreatedCrawlerStep.ExecuteStepAsync(currentStepCompleted.StepData,
                        completedStep.ExecutedStepsCollection).ConfigureAwait(false);
                }

                await CrawlFilterLogsAsync(currentStepCompleted).ConfigureAwait(false);
            }
        }


        protected virtual async Task CrawlFilterLogsAsync(CrawlerStepCompleted<TransactionReceiptVO> completedStep)
        {
            if (completedStep != null && FilterLogCrawlerStep.Enabled)
            {
                foreach (var log in completedStep.StepData.TransactionReceipt.Logs.ConvertToFilterLog())
                {
                    await CrawlFilterLogAsync(completedStep, log).ConfigureAwait(false);
                }
            }
        }

        protected virtual async Task CrawlFilterLogAsync(CrawlerStepCompleted<TransactionReceiptVO> completedStep, FilterLog filterLog)
        {
            if (FilterLogCrawlerStep.Enabled)
            {
                var currentStepCompleted = await FilterLogCrawlerStep.ExecuteStepAsync(
                    new FilterLogVO(completedStep.StepData.Transaction, completedStep.StepData.TransactionReceipt,
                        filterLog), completedStep.ExecutedStepsCollection).ConfigureAwait(false);
            }
        }

        public async Task ProcessAsync(BigInteger blockNumber, CancellationToken cancellationToken)
        {
            await CrawlBlockAsync(blockNumber).ConfigureAwait(false);
        }
    }
}
