using EntityFrameworkCore.EncryptColumn.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntityFrameworkCore.EncryptColumn.Converter
{
    internal sealed class EncryptionConverter(IEncryptionProvider encryptionProvider, ConverterMappingHints mappingHints = null) : ValueConverter<string, string>(x => encryptionProvider.Encrypt(x), x => encryptionProvider.Decrypt(x), mappingHints)
    {
    }
}
