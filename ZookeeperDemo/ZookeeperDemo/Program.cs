using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooKeeperNet;

namespace ZookeeperDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            //创建一个Zookeeper实例，第一个参数为目标服务器地址和端口，第二个参数为Session超时时间，第三个为节点变化时的回调方法 
            using (ZooKeeper zk = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 0, 0, 50000), new Watcher()))
            {
                var stat = zk.Exists("/root", true);
                if (stat != null)
                {

                    CreateOrUpdateNode(zk, "/root/childone", "childone", CreateMode.Persistent);
                    //取得/root节点下的子节点名称,返回List<string> 
                    var childrenNodes = zk.GetChildren("/root", true);
                    //取得/root/childone节点下的数据,返回string
                    var childrenData = BytesToString(zk.GetData("/root/childone", true, null));
                    //删除/root/childone这个节点，第二个参数为版本，－1的话直接删除，无视版本 
                    zk.Delete("/root/childone", -1);
                }
                else
                {
                    ////创建一个节点root，数据是mydata,不进行ACL权限控制，节点为永久性的(即客户端shutdown了也不会消失) 
                    zk.Create("/root", "mydata".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }

            }

        }

        private static void CreateOrUpdateNode(ZooKeeper zk, string path, string data, CreateMode mode)
        {
            var checkIsExist = zk.GetData(path, true, null);
            if (checkIsExist == null)
            {
                //在root下面创建一个childone znode,数据为childone,不进行ACL权限控制，节点为永久性的 
                zk.Create(path, data.GetBytes(), Ids.OPEN_ACL_UNSAFE, mode);
            }
            else
            {
                //修改节点/root/childone下的数据，第三个参数为版本，如果是-1，那会无视被修改的数据版本，直接改掉
                zk.SetData(path, data.GetBytes(), -1);
            }
        }
        private static string BytesToString(byte[] value)
        {
            if (value == null)
                return null;

            return Encoding.Default.GetString(value);
        }
    }
}
