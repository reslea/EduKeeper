[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EduKeeper.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(EduKeeper.Web.App_Start.NinjectWebCommon), "Stop")]

namespace EduKeeper.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Reflection;
    using Services;
    using Infrastructure;
    using EntityFramework;
    using EduKeeper.Services;
    using Infrastructure.ErrorUtilities;
    using Infrastructure.RepositoryInterfaces;
    using EntityFramework.Repositories;
    using Infrastructure.ServicesInretfaces;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<EduKeeperContext>().ToSelf().InRequestScope();
            
            kernel.Bind<IUserContext>().To<UserContext>();

            kernel.Bind<IErrorUtilities>().To<ErrorUtilities>();

            //repositories
            kernel.Bind<ICommentRepository>().To<CommentRepository>();
            kernel.Bind<ICourseRepository>().To<CourseRepository>();
            kernel.Bind<IFileRepository>().To<FileRepository>();
            kernel.Bind<IPostRepository>().To<PostRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();

            //entity services
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<ICourseService>().To<CourseService>();
            kernel.Bind<IFileService>().To<FileService>();
            kernel.Bind<IPostService>().To<PostService>();
            kernel.Bind<IUserService>().To<UserService>();
        }        
    }
}
