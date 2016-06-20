﻿namespace GuidGenTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// Wraps test cases for FactAttribute and TheoryAttribute so the test case runs in the STA Thread
    /// </summary>
    [DebuggerDisplay(@"\{ class = {TestMethod.TestClass.Class.Name}, method = {TestMethod.Method.Name}, display = {DisplayName}, skip = {SkipReason} \}")]
    public class STATestCase : LongLivedMarshalByRefObject, IXunitTestCase
    {
        private IXunitTestCase testCase;

        public STATestCase(IXunitTestCase testCase)
        {
            this.testCase = testCase;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", error: true)]
        public STATestCase()
        {
        }

        public IMethodInfo Method
        {
            get { return this.testCase.Method; }
        }

        public string DisplayName
        {
            get { return this.testCase.DisplayName; }
        }

        public string SkipReason
        {
            get { return this.testCase.SkipReason; }
        }

        public ISourceInformation SourceInformation
        {
            get { return this.testCase.SourceInformation; }
            set { this.testCase.SourceInformation = value; }
        }

        public ITestMethod TestMethod
        {
            get { return this.testCase.TestMethod; }
        }

        public object[] TestMethodArguments
        {
            get { return this.testCase.TestMethodArguments; }
        }

        public Dictionary<string, List<string>> Traits
        {
            get { return this.testCase.Traits; }
        }

        public string UniqueID
        {
            get { return this.testCase.UniqueID; }
        }

        public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            var tcs = new TaskCompletionSource<RunSummary>();
            var thread = new Thread(() =>
            {
                try
                {
                    // Set up the SynchronizationContext so that any awaits
                    // resume on the STA thread as they would in a GUI app.
                    SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());

                    // Start off the test method.
                    var testCaseTask = this.testCase.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);

                    // Arrange to pump messages to execute any async work associated with the test.
                    var frame = new DispatcherFrame();
                    Task.Run(async delegate
                    {
                        try
                        {
                            await testCaseTask;
                        }
                        finally
                        {
                            // The test case's execution is done. Terminate the message pump.
                            frame.Continue = false;
                        }
                    });
                    Dispatcher.PushFrame(frame);

                    // Report the result back to the Task we returned earlier.
                    CopyTaskResultFrom(tcs, testCaseTask);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
        {
            this.testCase = info.GetValue<IXunitTestCase>(nameof(testCase));
        }

        void IXunitSerializable.Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(testCase), this.testCase);
        }

        private static void CopyTaskResultFrom<T>(TaskCompletionSource<T> tcs, Task<T> template)
        {
            if (tcs == null)
            {
                throw new ArgumentNullException(nameof(tcs));
            }

            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (!template.IsCompleted)
            {
                throw new ArgumentException("Task must be completed first.", nameof(template));
            }

            if (template.IsFaulted)
            {
                tcs.SetException(template.Exception);
            }
            else if (template.IsCanceled)
            {
                tcs.SetCanceled();
            }
            else
            {
                tcs.SetResult(template.Result);
            }
        }
    }
}
