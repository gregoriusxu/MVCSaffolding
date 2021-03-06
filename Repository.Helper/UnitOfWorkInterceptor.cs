//using System.Threading.Tasks;
//using Castle.DynamicProxy;
//using Repository.Pattern.Ef6;

//namespace Abp.Domain.Uow
//{
//    /// <summary>
//    /// This interceptor is used to manage database connection and transactions.
//    /// </summary>
//    internal class UnitOfWorkInterceptor : IInterceptor
//    {
//        private readonly IUnitOfWorkManager _unitOfWorkManager;

//        public UnitOfWorkInterceptor(IUnitOfWorkManager unitOfWorkManager)
//        {
//            _unitOfWorkManager = unitOfWorkManager;
//        }

//        /// <summary>
//        /// Intercepts a method.
//        /// </summary>
//        /// <param name="invocation">Method invocation arguments</param>
//        public void Intercept(IInvocation invocation)
//        {
//            if (_unitOfWorkManager.Current != null)
//            {
//                //Continue with current uow
//                invocation.Proceed();
//                return;
//            }

//            var unitOfWorkAttr = UnitOfWorkAttribute.GetUnitOfWorkAttributeOrNull(invocation.MethodInvocationTarget);
//            if (unitOfWorkAttr == null || unitOfWorkAttr.IsDisabled)
//            {
//                //No need to a uow
//                invocation.Proceed();
//                return;
//            }

//            //No current uow, run a new one
//            PerformUow(invocation, unitOfWorkAttr.CreateOptions());
//        }

//        private void PerformUow(IInvocation invocation, UnitOfWorkOptions options)
//        {
//            if (
//                invocation.Method.ReturnType == typeof(Task) ||
//                (invocation.Method.ReturnType.IsGenericType && invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
//                )
//            {
//                PerformAsyncUow(invocation, options);
//            }
//            else
//            {
//                PerformSyncUow(invocation, options);
//            }
//        }

//        private void PerformSyncUow(IInvocation invocation, UnitOfWorkOptions options)
//        {
//            using (var uow = _unitOfWorkManager.Begin(options))
//            {
//                invocation.Proceed();
//                uow.Complete();
//            }
//        }

//        private void PerformAsyncUow(IInvocation invocation)
//        {
//            var uow = new UnitOfWork();

//            invocation.Proceed();

//            if (invocation.Method.ReturnType == typeof(Task))
//            {
//                invocation.ReturnValue = InternalAsyncHelper.WaitTaskAndActionWithFinally(
//                    (Task)invocation.ReturnValue,
//                    async () => await uow.CompleteAsync(),
//                    uow.Dispose
//                    );
//            }
//            else //Task<TResult>
//            {
//                invocation.ReturnValue = InternalAsyncHelper.CallReturnGenericTaskAfterAction(
//                    invocation.Method.ReturnType.GenericTypeArguments[0],
//                    invocation.ReturnValue,
//                    async () => await uow.CompleteAsync(),
//                    uow.Dispose
//                    );
//            }
//        }
//    }
//}