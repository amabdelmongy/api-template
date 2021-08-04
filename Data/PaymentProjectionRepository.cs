using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;
using Domain;
using Domain.Payment.Projection;

namespace Data
{
    public class PaymentProjectionRepository : IPaymentProjectionRepository
    {
        private const string TableName = "[PaymentProjections]";
        private readonly string _connectionString;

        public PaymentProjectionRepository(
            string connectionString
        )
        {
            _connectionString = connectionString;
        }

        public Result<PaymentProjection> Get(Guid id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE PaymentId = '{id}';";
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var payments = connection.QueryFirstOrDefault<PaymentProjection>(sql);
                return Result.Ok(payments);
            }
            catch (Exception ex)
            {
                return Result.Failed<PaymentProjection>(Error.CreateFrom("PaymentProjection", ex));
            }
        }
        public Result<IEnumerable<PaymentProjection>> Get()
        {
            var sql = $"SELECT * FROM {TableName};";
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var payments = connection.Query<PaymentProjection>(sql);
                return Result.Ok(payments);
            }
            catch (Exception ex)
            {
                return Result.Failed<IEnumerable<PaymentProjection>>(Error.CreateFrom("PaymentProjection", ex));
            }
        }

        public Result<object> Add(PaymentProjection paymentProjection)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                    connection.Insert(paymentProjection);

                return Result.Ok<object>();
            }
            catch (Exception ex)
            {
                return
                    Result.Failed<object>(
                        Error.CreateFrom("Error when Adding to PaymentProjection", ex)
                    );
            }
        }
    }
}
