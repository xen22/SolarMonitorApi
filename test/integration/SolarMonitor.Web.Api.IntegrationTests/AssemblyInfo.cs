using Xunit;

// Note: disable test parallelization for now until I figure out how to use different in-memory databases
// for each integration test.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
