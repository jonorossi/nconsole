//using System;
//using System.Collections.Generic;
//
//namespace NConsole.Tests
//{
//    public class NewApi
//    {
        //TODO: NConsole needs to support large apps (e.g. bricks, git, svn), but still be simple enough to support
        //      small apps that want full control of the args without commands

        //TODO: Work out how you would still use NConsole for an app like ls

        //TODO: Need support for nested commands (for now I think a single level is good enough):
        //    git svn clone http://svn.foo.org/project -T trunk -b branches -t tags
        //    git svn dcommit
        //    git remote add origin git@github.com:user/repo.git
        //    git stash apply
        //    bricks cert [add|remove|list]

        //Single level command with args:
        //bricks install xyz
        //bricks install xyz abc
        //bricks search ab
        //bricks help install
//
//        [Command("cert")]
//        public class CertificateCommand : ICommand
//        {
//            // The convention here is that if the method is called Execute, then it is the default. Or should this be
//            // controlled by attributes.
//            public void Execute()
//            {
//            }
//
//            public void List()
//            {
//            }
//
//            public void Add()
//            {
//            }
//
//            public void Remove()
//            {
//            }
//        }

//        public void Design()
//        {
//            string[] args = new string[] { };
//
//            var registry = new CommandRegistry();
//            registry.Register(typeof(CloneCommand));
//            registry.Register<CommitCommand>();
//            registry.Register<MoveCommand>();
//            registry.Register(Assembly.GetExecutingAssembly().GetTypes()
//                .Where(x => x is ICommand)
//                .Where(x => x.IsPublic));
//
//            var controller = new ConsoleController();
//            controller.Mode = Mode.Posix;
//
//            // Optional for using your own factory (e.g. Windsor)
//            // Need to work out some way that the default factory is already available for use
//            controller.CommandFactory = new WindsorCommandFactory(registry, null);
//            controller.CommandFactory = new DefaultCommandFactory(registry);
//
//            // If you want to set what happens when no command is specified
//            controller.DefaultCommand = typeof(HelpCommand);
//
//            // Start the program
//            int returnCode = controller.Execute(args);
//        }


//    }

//    public class WindsorCommandFactory : ICommandFactory
//    {
//        private readonly ICommandRegistry commandRegistry;
//        private readonly IKernel kernel;
//
//        public WindsorCommandFactory(ICommandRegistry commandRegistry, IKernel kernel)
//        {
//            this.commandRegistry = commandRegistry;
//            this.kernel = kernel;
//        }
//
//        public ICommand Create(string commandName)
//        {
//            Type type = commandRegistry.GetCommand(commandName);
//
//            return kernel.Resolve(type) as ICommand;
//        }
//
//        public void Release(ICommand command)
//        {
//            kernel.Release(command);
//        }
//    }

//    public interface IKernel
//    {
//        object Resolve(Type type);
//        void Release(object instance);
//    }

    // Actions in commands don't need an attribute because it should be convention over configuration;
    // they are public which is the convention.

    // HelpCommand is basically a HelpController

//    [Command(Description = "Clone a repository into a new directory")]
//    public class CloneCommand : ICommand
//    {
//        public void Execute(
//            [Option]
//            string repository,
//            [Option]
//            CloneOptions options)
//        {
//        }
//
//        [DefaultCommand]
//        public void Execute(Uri repository, CloneOptions options)
//        {
//        }
//
//        public void Execute(Uri repository, DirectoryInfo directory, CloneOptions options)
//        {
//        }
//
//        public class CloneOptions : GlobalOptions
//        {
//            [Option(ShortName = "q", LongName = "quiet")]
//            public bool Quiet { get; set; }
//        }
//    }

//    public class GlobalOptions
//    {
//        [Option(ShortName = "h", LongName = "help")]
//        public bool SomethingGlobal { get; set; }
//    }
//
//    public class OptionAttribute : Attribute
//    {
//        public string ShortName { get; set; }
//        public string LongName { get; set; }
//    }

//    public class CommitCommand : ICommand
//    {
//    }
//
//    [Command("mv")]
//    public class MoveCommand : ICommand
//    {
//    }

//    public class CommandAttribute : Attribute
//    {
//        public CommandAttribute()
//        {
//        }
//
//        public CommandAttribute(string name)
//        {
//        }
//
//        public string Description { get; set; }
//    }
//
//    public enum Mode
//    {
//        Undefined,
//        Windows,
//        //TODO: Is it POSIX???
//        Posix
//    }

//    public class DefaultCommandFactory : ICommandFactory
//    {
//        private readonly ICommandRegistry commandRegistry;
//
//        public DefaultCommandFactory(ICommandRegistry commandRegistry)
//        {
//            this.commandRegistry = commandRegistry;
//        }
//
//        public ICommand Resolve(string commandName)
//        {
//            return Activator.CreateInstance(commandRegistry.GetCommand(commandName)) as ICommand;
//        }
//
//        public void Release(ICommand command)
//        {
//        }
//    }

//    public interface ICommandRegistry
//    {
//        void Register(Type type);
//        void Register(IEnumerable<Type> types);
//        void Register<T>() where T : ICommand;
//        Type GetCommand(string commandName);
//    }
//
//    internal class CommandRegistry : ICommandRegistry
//    {
//        public void Register(Type type)
//        {
//        }
//
//        public void Register(IEnumerable<Type> types)
//        {
//        }
//
//        public void Register<T>() where T : ICommand
//        {
//        }
//
//        public Type GetCommand(string commandName)
//        {
//            return null;
//        }
//    }
//}

//========================================================================================

//namespace NConsoleSandbox.Idea2
//{
//    // ===== Common Base ======
//
//    public abstract class AppCommandBase : ICommand
//    {
//        [Argument(ShortName = "q", HelpMessage = "Operate quietly.")]
//        public bool Quiet { get; set; }
//
//        public abstract void Execute();
//    }
//
//    // ===== Clone ======
//    // Example: git clone git://git.kernel.org/pub/scm/.../linux-2.6 my2.6
//
//    [Command("clone")]
//    public class CloneCommand : AppCommandBase
//    {
//        [Argument(Position = 0, Mandatory = true, HelpMessage = "The (possibly remote) repository to clone from. See the URLS section below for more information on specifying repositories.")]
//        public Uri Repository { get; set; }
//
//        [Argument(Position = 1, HelpMessage = "The name of a new directory to clone into. The \"humanish\" part of the source repository is used if no directory is explicitly given (\"repo\" for \"/path/to/repo.git\" and \"foo\" for \"host.xz:foo/.git\"). Cloning into an existing directory is only allowed if the directory is empty.")]
//        public DirectoryInfo Directory { get; set; }
//
//        public override void Execute()
//        {
//        }
//    }
//
//    // ===== Stash ======
//    // git stash foobar
//    // git stash pop foobar
//
//    [Command("stash", HelpMessage = "Stash the changes in a dirty working directory away.")]
//    [SubCommand(typeof(StashSaveCommand), Default = true)]
//    [SubCommand(typeof(StashPopCommand))]
//    public abstract class StashCommand : AppCommandBase { }
//
//    [Command("save", HelpMessage = "Save your local modifications to a new stash.")]
//    public class StashSaveCommand : AppCommandBase
//    {
//        [Argument(Position = 0)]
//        public string Stash { get; set; }
//
//        [Argument]
//        public bool KeepIndex { get; set; }
//
//        public override void Execute()
//        {
//        }
//    }
//
//    [Command("pop", HelpMessage = "Remove a single stashed state from the stash list and apply it on top of the current working tree state.")]
//    public class StashPopCommand : AppCommandBase
//    {
//        [Argument(Position = 0)]
//        public string Stash { get; set; }
//
//        public override void Execute()
//        {
//        }
//    }
//}