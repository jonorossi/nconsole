using System;
using System.Linq;

namespace NConsole
{
    /// <summary>
    /// Provides static methods and properties to manage a console application, such as methods to start
    /// an application. This class cannot be inherited.
    /// </summary>
    public static class ConsoleApplication
    {
        /// <summary>
        /// Begins running a standard console application on the current thread, and executes the
        /// <see cref="ICommand"/> specified by <paramref name="applicationCommand"/> type.
        /// </summary>
        /// <param name="args">The command line arguments passed by the user of the application.</param>
        /// <param name="applicationCommand">The <see cref="ICommand"/> that will be executed if this console
        /// application does not support commands.</param>
        /// <returns>The exit code of the executing the specified command</returns>
        public static int Run(string[] args, Type applicationCommand)
        {
            try
            {
                ConsoleController controller = new ConsoleController();
                controller.Register(applicationCommand);
                controller.SetDefaultCommand(applicationCommand);
                return controller.Execute(args);
            }
            catch (Exception ex)
            {
                //TODO: remove this stuff:
                if (args.Any(a => a == "--nconsole-debug"))
                {
                    Console.Error.WriteLine(ex);
                }
                else
                {
                    Console.Error.WriteLine(ex.Message);
                }
                return 1;
            }
        }

        //Type helpCommand = ...built in help command

//        public static int Run(string[] args, ArgumentMode mode)
//        {
//            throw new NotImplementedException();
//        }
    }
}