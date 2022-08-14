using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpDataOriented;

public record PropSeq(string Name, IEnumerable<PropSeq> Complex = null, object Primitive = null);

public delegate PropSeq Sequencer<T>(T obj);

public delegate PropSeq Sequencer(object obj);
