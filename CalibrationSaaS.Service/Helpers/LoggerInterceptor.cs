using CalibrationSaaS.Domain.BusinessExceptions.WorkOrderDetail;
using CalibrationSaaS.Domain.BusinessExceptions.Customer;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CalibrationSaaS.Domain.BusinessExceptions;
using CalibrationSaaS.Domain.Aggregates.Querys;

namespace CalibrationSaaS.Infraestructure.Grpc.Helpers
{
    public class LoggerInterceptor : Interceptor
    {
        private readonly ILogger<LoggerInterceptor> _logger;

        public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
        {
            _logger = logger;
        }

        public async override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            LogCall(context);
            try
            {
                return await continuation(request, context);
            }
            catch (InvalidCalSaaSModel ex1)
            {
                _logger.LogError(ex1.Message);
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex1.Message));
            }
            catch (ExistingException ex)
            {
                var message = string.Join(' ', ex.Message);
                _logger.LogError(message);
                throw new RpcException(new Status(StatusCode.AlreadyExists, message));
            }
            catch (ExistingCustomerException ex)
            {
                var message = string.Join(' ', ex.Message, ex.CustomerName);
                _logger.LogError(message);
                throw new RpcException(new Status(StatusCode.AlreadyExists, message));
            }
            catch (CalibrationSaaS.Domain.BusinessExceptions.WorkOrderDetail.ChangeStatusException ex)
            {
                var message = string.Join(' ', ex.Message, ex.Id);
                _logger.LogError(message);
                throw new RpcException(new Status(StatusCode.FailedPrecondition, message)); 
            }
            catch (CalibrationSaaS.Domain.BusinessExceptions.WorkOrderDetail.CalculateException ex)
            {
                var message = string.Join(' ', ex.Message, ex.Id);
                _logger.LogError(message);
                throw new RpcException(new Status(StatusCode.InvalidArgument, message));
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Aborted, ex.Message));
            }
            catch (CertificationException ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
             catch (SchemaException ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
            catch (AlreadyInUseException ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }
            catch (LoginException ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
            }

            catch (Exception e)
            {
                if (e.Message.Contains("|"))
                {
                    throw new RpcException(new Status(StatusCode.FailedPrecondition, e.Message));
                }
                else
                if (e.InnerException != null && e.InnerException.Message  != null 
                    && e.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    var message = "The DELETE statement conflicted with the REFERENCE constraint";
                    throw new RpcException(new Status(StatusCode.FailedPrecondition, message));
                }
                else if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e, $"An error occured when calling {context.Method}");
                    throw new RpcException(new Status(StatusCode.FailedPrecondition, e.Message));
                }
                else
                {
                    _logger.LogError(e, $"An error occured when calling {context.Method}");
                    throw new RpcException(Status.DefaultCancelled, e.Message);
                }
            }
            

        }

        private void LogCall(ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            _logger.LogDebug($"Starting call. Request: {httpContext.Request.Path}");
        }
    }
}
