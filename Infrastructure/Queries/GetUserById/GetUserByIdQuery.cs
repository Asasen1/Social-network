using Application.Abstractions;
using Application.DTO;
using Dapper;
using Domain.Common;
using kp.Dapper.Handlers;

namespace Infrastructure.Queries.GetUserById
{
    public class GetUserByIdQuery : IQueryHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        private readonly SqlConnectionFactory _factory;

        public GetUserByIdQuery(SqlConnectionFactory factory)
        {
            SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
            _factory = factory;
        }

        public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdRequest request, CancellationToken ct)
        {
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
                       WHERE Id = @userId
                       """;
            var user = await connection.QueryFirstOrDefaultAsync<UserDto>
                (sql, new { userId = request.Id });
            if (user is null)
                return Errors.General.NotFound(request.Id);
            
            return new GetUserByIdResponse(user);
        }
    }
}