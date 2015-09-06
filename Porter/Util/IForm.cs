using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Porter.Util
{
    public interface IForm<TForm, TRecord>
    {
        void From(int RecordID);
        void From(TRecord Item);
        int Insert();
        void Update(int RecordID);
    }
}
