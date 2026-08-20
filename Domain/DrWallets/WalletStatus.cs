using NpgsqlTypes;

namespace TestCI.Domain.DrWallets
{
    public enum StatusWallet
    {
        [PgName("Prcs")]
        Prcs,

        [PgName("Actv")]
        Actv,

        [PgName("Blck")]
        Blck,

        [PgName("Clsd")]
        Clsd
    }
}