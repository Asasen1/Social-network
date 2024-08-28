using Application.Abstractions;
using Application.DTO;
using Dapper;
using Domain.Common;
using kp.Dapper.Handlers;

namespace Infrastructure.Queries.GetAllUsers;

public class GetAllUsersQuery : IQueryHandler<GetAllUsersRequest, GetAllUsersResponse>
{
    private readonly SqlConnectionFactory _factory;

    public GetAllUsersQuery(SqlConnectionFactory factory)
    {
        SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
        _factory = factory;
    }
    public async Task<Result<GetAllUsersResponse>> Handle(GetAllUsersRequest request, CancellationToken ct)
    {
        if (request.Page <= 0)
            return Errors.General.ValueIsInvalid(nameof(request.Page));
        using var connection = _factory.CreateConnection();
        connection.Open();
        var sql = $"""
                   SELECT
                   u.id AS Id,
                   u.first_name AS {nameof(UserDto.FirstName)},
                   u.second_name AS {nameof(UserDto.SecondName)},
                   u.nickname AS {nameof(UserDto.Nickname)},
                   u.description AS {nameof(UserDto.Description)},
                   u.birth_date AS {nameof(UserDto.BirthDate)}
                   FROM users AS u
                   OFFSET @skip
                   LIMIT @take
                   """;
        var users = await connection.QueryAsync<UserDto>
            (sql, new{skip = (request.Page-1)*request.Count, take = request.Count});
        return new GetAllUsersResponse(users.ToList());
    }
}