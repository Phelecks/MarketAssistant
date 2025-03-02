namespace LoggerService.Helpers
{
    /// <summary>
    /// System events
    /// </summary>
    public enum EventType
    {
        General = 0,
        Performance = 1,

        /// <summary>
        /// Cache manager events
        /// </summary>
        CacheManager = 2000,
        /// <summary>
        /// Local cache
        /// </summary>
        LocalCache = 2001,

        /// <summary>
        /// Identity
        /// </summary>
        Identity = 3000,
        /// <summary>
        /// Identity exception
        /// </summary>
        IdentityException = 3001,
        /// <summary>
        /// Identity background task
        /// </summary>
        IdentityBackgroundTasks = 3002,

        /// <summary>
        /// Informing
        /// </summary>
        Informing = 4000,
        /// <summary>
        /// Informing exception
        /// </summary>
        InformingException = 4001,
        /// <summary>
        /// Informing background task
        /// </summary>
        InformingBackgroundTasks = 4002,

        /// <summary>
        /// Kernel
        /// </summary>
        Kernel = 5000,
        /// <summary>
        /// Kernel exception
        /// </summary>
        KernelException = 5001,
        /// <summary>
        /// Kernel background task
        /// </summary>
        KernelBackgroundTasks = 5002,

        /// <summary>
        /// Catalog
        /// </summary>
        Catalog = 6000,
        /// <summary>
        /// Catalog exception
        /// </summary>
        CatalogException = 6001,
        /// <summary>
        /// Catalog background task
        /// </summary>
        CatalogBackgroundTasks = 6002,

        /// <summary>
        /// Customer
        /// </summary>
        Customer = 7000,
        /// <summary>
        /// Customer exception
        /// </summary>
        CustomerException = 7001,
        /// <summary>
        /// Customer background tasks
        /// </summary>
        CustomerBackgroundTasks = 7002,

        /// <summary>
        /// Order
        /// </summary>
        Order = 8000,
        /// <summary>
        /// Order exception
        /// </summary>
        OrderException = 8001,
        /// <summary>
        /// Order background tasks
        /// </summary>
        OrderBackgroundTasks = 8002,

        /// <summary>
        /// Financial
        /// </summary>
        Financial = 9000,
        /// <summary>
        /// Financial exception
        /// </summary>
        FinancialException = 9001,
        /// <summary>
        /// Financial background tasks
        /// </summary>
        FinancialBackgroundTasks = 9002,

        /// <summary>
        /// Messaging
        /// </summary>
        Messaging = 12000,
        /// <summary>
        /// Messaging exception
        /// </summary>
        MessagingException = 12001,
        /// <summary>
        /// Messaging background tasks
        /// </summary>
        MessagingBackgroundTasks = 12002,

        /// <summary>
        /// Customer
        /// </summary>
        CustomerClub = 13000,
        /// <summary>
        /// Customer exception
        /// </summary>
        CustomerClubException = 13001,
        /// <summary>
        /// Customer background tasks
        /// </summary>
        CustomerClubBackgroundTasks = 13002,


        /// <summary>
        /// Basket
        /// </summary>
        Basket = 14000,
        /// <summary>
        /// Basket exception
        /// </summary>
        BasketException = 14001,
        /// <summary>
        /// Basket background tasks
        /// </summary>
        BasketBackgroundTasks = 14002,

        /// <summary>
        /// Block chain
        /// </summary>
        BlockChain = 15000,
        /// <summary>
        /// Block chain exception
        /// </summary>
        BlockChainException = 15001,
        /// <summary>
        /// Block chain background tasks
        /// </summary>
        BlockChainBackgroundTasks = 15002,
        /// <summary>
        /// Block chain
        /// </summary>
        BlockChainLogProcessor = 15003,
        /// <summary>
        /// Block chain exception
        /// </summary>
        BlockChainLogProcessorException = 15004,
        /// <summary>
        /// Block chain background tasks
        /// </summary>
        BlockChainLogProcessorBackgroundTasks = 15005,
        /// <summary>
        /// Block chain
        /// </summary>
        BlockChainTransfer = 15006,
        /// <summary>
        /// Block chain exception
        /// </summary>
        BlockChainTransferException = 15007,
        /// <summary>
        /// Block chain background tasks
        /// </summary>
        BlockChainTransferBackgroundTasks = 15008,
        /// <summary>
        /// Block chain
        /// </summary>
        BlockProcessor = 15009,
        /// <summary>
        /// Block chain exception
        /// </summary>
        BlockProcessorException = 15010,
        /// <summary>
        /// Block chain background tasks
        /// </summary>
        BlockProcessorBackgroundTasks = 15011,

        /// <summary>
        /// Contract
        /// </summary>
        Contract = 16000,
        /// <summary>
        /// Contract exception
        /// </summary>
        ContractException = 16001,
        /// <summary>
        /// Contract background tasks
        /// </summary>
        ContractBackgroundTasks = 16002,

        /// <summary>
        /// Contract
        /// </summary>
        Game = 17000,
        /// <summary>
        /// Contract exception
        /// </summary>
        GameException = 17001,
        /// <summary>
        /// Contract background tasks
        /// </summary>
        GameBackgroundTasks = 17002,


        Orchestrator = 20000
    }

    /// <summary>
    /// Event tool
    /// </summary>
    public static class EventTool
    {
        /// <summary>
        /// Get event
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static Microsoft.Extensions.Logging.EventId GetEventInformation(EventType eventType, string eventName = null)
        {
            return new Microsoft.Extensions.Logging.EventId((int)eventType, string.IsNullOrEmpty(eventName) ? eventType.ToString() : eventName);
        }
    }
}
