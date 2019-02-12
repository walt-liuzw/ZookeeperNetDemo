using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace ZookeeperDemo
{
    class Watcher : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            if (@event.Type == EventType.NodeDataChanged)
            {
                Console.WriteLine("Node节点下数据发生改变,已经触发了" + @event.Type + "事件！");
            }
        }
    }
}
