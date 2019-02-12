using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZookeeperDemo
{
    public class RetryHelper
    {
        private int retryDelay = 500;
        private long signal = 0;
        public Action IfErrorThen;
        public Action CreateNodeStructure;
        public Action FixConnectionLossAction;

        public static RetryHelper Make()
        {
            return new RetryHelper();
        }

        public void Execute(Action action)
        {
            while (true)
            {
                try
                {
                    action();
                    break;
                }
                catch (ZooKeeperNet.KeeperException.NoNodeException ex)
                {
                    //create node structure

                    Console.WriteLine("retry helper NoNodeException: " + ex.Message);

                    if (CreateNodeStructure != null)
                        RetryHelper.Make().Execute(CreateNodeStructure);
                    continue;
                }
                catch (ZooKeeperNet.KeeperException.ConnectionLossException ex)
                {
                    Console.WriteLine("retry helper ConnectionLossException: " + ex.Message);

                    long attempSignal = Interlocked.Read(ref signal);

                    while (Interlocked.Read(ref signal) > 0)
                        Thread.Sleep(retryDelay);

                    if (attempSignal == 0)
                    {
                        Interlocked.Increment(ref signal);

                        if (FixConnectionLossAction != null)
                            RetryHelper.Make().Execute(FixConnectionLossAction);

                        Interlocked.Decrement(ref signal);
                    }

                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("retry helper catch: " + ex.Message);
                    Thread.Sleep(retryDelay);

                    if (IfErrorThen != null)
                        IfErrorThen();
                    continue;
                }
            }
        }
    }
}
