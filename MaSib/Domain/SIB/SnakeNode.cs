//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MaSib.Algorithms;

//namespace MaSib.Domain.SIB
//{
//    public class SnakeNode : INode
//    {
//        [DebuggerDisplay("{snake.world.IntArrToBitString(snake.tail),nq}")]
//        public Snake snake { get; }

//        public SnakeNode(Snake s)
//        {
//            this.snake = s;
//        }

//        public ListType ListType { get; set; }
//        public int f {
//            get { return snake.f; }
//            }
//        public int h {
//            get { return snake.h; }
//        }

//        public int g
//        {
//            get { return snake.g; }
//        }
//        public INode Parent { get; set; }
//        public LinkedList<INode> Children { get; private set; }

//        public void generate()
//        {
//            var childrens = snake.generate();
//            Children = new LinkedList<INode>();
//            foreach (var c in childrens)
//            {
//                Children.AddLast(new SnakeNode(c));
//            }
//        }

//        public override string ToString()
//        {
//            return snake.ToString();
//        }

//    }
//}
