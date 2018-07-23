using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Orleans.Core")]
[assembly: InternalsVisibleTo("Orleans.Runtime")]
[assembly: InternalsVisibleTo("Orleans.TestingHost")]
[assembly: InternalsVisibleTo("Orleans.TestingHost.AppDomain")]
[assembly: InternalsVisibleTo("OrleansManager")]

[assembly: InternalsVisibleTo("DefaultCluster.Tests")]
[assembly: InternalsVisibleTo("NonSilo.Tests")]
[assembly: InternalsVisibleTo("Tester.AzureUtils")]
[assembly: InternalsVisibleTo("AWSUtils.Tests")]
[assembly: InternalsVisibleTo("TesterInternal")]
[assembly: InternalsVisibleTo("TestInternalGrainInterfaces")]
[assembly: InternalsVisibleTo("TestInternalGrains")]
[assembly: InternalsVisibleTo("Youscan.MentionStream.Cluster")]

// Legacy provider support
[assembly: InternalsVisibleTo("Orleans.Core.Legacy")]
[assembly: InternalsVisibleTo("Orleans.Runtime.Legacy")]
