using Application.Abstractions;
using Application.DTO;
using Dapper;
using Domain.Agregates;
using Domain.Common;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Queries.GetUserById
{
    public class GetUserByIdQuery : IQueryHandler<GetUserByIdRequest, GetUserByIdResponse>
    {
        private readonly SqlConnectionFactory _factory;

        public GetUserByIdQuery(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdRequest request, CancellationToken ct)
        {
            using var connection = _factory.CreateConnection();
            connection.Open();

            var sql = @"
                     SELECT u.id AS Id,
    u.first_name AS FirstName,
    u.second_name AS SecondName,
    u.nickname AS Nickname,
    u.birth_date AS BirthDate,
    u.description AS Description,
    ph.id AS PhotoId,
    ph.path AS PhotoPath,
    ph.is_main AS IsMain,
    ph.user_id AS UserId
FROM users u
LEFT JOIN user_photos ph ON u.id = ph.user_id
WHERE u.id = @id AND ph.is_main = true

                      ";

            var users = await connection.QueryAsync<UserDto, PhotoDto, UserDto>(
                sql,
                (user, photo) =>
                {
                    user.Avatar = photo;
                    return user;
                }, splitOn: "id", param: new { id = request.Id });
            var user = users.FirstOrDefault();
            return new GetUserByIdResponse(user);
        }
    }
}