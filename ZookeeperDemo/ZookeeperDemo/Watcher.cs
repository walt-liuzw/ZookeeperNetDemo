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
            switch (@event.Type)
            {
                case EventType.NodeChildrenChanged:
                    Console.WriteLine("ZNode下子ZNode改变触发了" + @event.Type + "事件！");
                    break;
                case EventType.NodeCreated:
                    Console.WriteLine("新建ZNode触发了" + @event.Type + "事件！");
                    break;
                case EventType.NodeDataChanged:
                    Console.WriteLine("ZNode节点下数据发生改变触发了" + @event.Type + "事件！");
                    break;
                case EventType.NodeDeleted:
                    Console.WriteLine("删除ZNode节点经触发了" + @event.Type + "事件！");
                    break;
                default:
                    Console.WriteLine("没有任何改变触发了" + @event.Type + "事件！");
                    break;
            }
        }
    }
}
