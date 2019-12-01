using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LogSample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private string _FullPath;
        /// <summary>
        /// ログファイルを置く場所.
        /// </summary>
        public string FullPath
        {
            get { return _FullPath; }
            set { _FullPath = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 初期化.
        /// 本来はApp.xaml.csなどに書くが実験のためここに書く.
        /// </summary>
        public Command CommandInit => new Command((args) =>
        {
            // ファイル出力のパスはフルパスで指定しなければならない.
            FullPath = FileSystem.AppDataDirectory;
            System.Diagnostics.Debug.WriteLine($"---- {FullPath}");
            var fileName = System.IO.Path.Combine(FullPath, "all.log");

            // NLog.config相当の内容をコードで書く.
            var config = new NLog.Config.LoggingConfiguration();

            // UWPにはコンソールが存在しないため出力されない.
            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = fileName };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            // クラス毎にロガーを作るためにファクトリーを用意する.
            // ここでは全ログを渡してNLogのRuleでフィルターする方式にしてみる.
            // 実際にはTraceは出ない.別の理由がありそう.
            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                .AddFilter("", LogLevel.Trace)
                .AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
            });

            // ロガーを作る.
            logger = loggerFactory.CreateLogger<MainPage>();
        });

        /// <summary>
        /// ロガーファクトリー.
        /// 本来は各クラスから参照できる場所に用意する.
        /// </summary>
        ILoggerFactory loggerFactory;

        /// <summary>
        /// ロガー.
        /// このクラスでログ出力するときに使うインスタンス.
        /// </summary>
        ILogger logger;

        /// <summary>
        /// ログ出力.
        /// </summary>
        public Command CommandLog => new Command((args) =>
        {
            // 重要度が高い順
            switch (args)
            {
                case "Critical":
                    logger.LogCritical($"Critical from ILogger.");
                    break;

                case "Error":
                    logger.LogError($"Error from ILogger.");
                    break;

                case "Warning":
                    logger.LogWarning($"Warning from ILogger.");
                    break;

                case "Information":
                    logger.LogInformation($"Information from ILogger.");
                    break;

                case "Debug":
                    logger.LogDebug($"Debug from ILogger.");
                    break;

                case "Trace":
                    logger.LogTrace($"Trace from ILogger.");
                    break;

                default:
                    System.Diagnostics.Debug.WriteLine($"Unknown {args}");
                    break;
            }
        });

    }
}
