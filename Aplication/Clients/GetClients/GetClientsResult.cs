namespace TestCI.Aplication.Clients.GetClients
{
    public class GetClientsResult
    {
        public List<ClientResult> Clients { get; set; } = new();

        public int TotalCount { get; set; }
    }

    public class ClientResult
    {
        public Guid Mid { get; set; }

        public string LastName { get; set; } = null!;

        public string FisrtName { get; set; } = null!;

        public string MiddleName { get; set; } = null!;

        public Guid? IdDr { get; set; }
    }
}
