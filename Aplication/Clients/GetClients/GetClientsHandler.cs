using TestCI.Aplication.Clients;
using TestCI.Aplication.Clients.GetClients;

namespace TestCI.Application.Clients.GetClients;

public class GetClientsHandler
{
    private readonly IClientRepository _clients;

    public GetClientsHandler(IClientRepository clients)
    {
        _clients = clients;
    }

    public async Task<GetClientsResult> Handle(
        GetClientsRequest request)
    {
        var clients = await _clients.Get(
            request.Search,
            request.Page,
            request.PageSize);

        var totalCount = await _clients.Count(
            request.Search);

        var result = new GetClientsResult
        {
            TotalCount = totalCount
        };

        foreach (var client in clients)
        {
            result.Clients.Add(new ClientResult
            {
                Mid = client.Mid,
                LastName = client.LastName,
                FisrtName = client.FisrtName,
                MiddleName = client.MiddleName,
                IdDr = client.IdDr
            });
        }

        return result;
    }
}