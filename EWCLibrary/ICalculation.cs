using System.Collections;
using System.Data;

namespace EWCLibrary
{
    public interface ICalculation
    {
        /// <summary>
        /// Unified recursive calculation of Data normalization, entropy quantization, Weight value, and Development index for multiple samples
        /// </summary>
        /// <param name="ht_original_able"></param>
        /// <param name="ht_gyh"></param>
        /// <param name="count"></param>
        void calculation_process(Hashtable ht_original_able, Hashtable ht_gyh, int count);
        /// <summary>
        /// Calculate the average value
        /// </summary>
        /// <param name="ht_original_able"></param>
        DataTable calculation_avg(Hashtable ht_original_able);
        /// <summary>
        /// calculation data normalization
        /// </summary>
        /// <param name="ht_original_able"></param>
        Hashtable calculation_gyh(Hashtable ht_original_able);
        /// <summary>
        /// Judging the progress of the process
        /// </summary>
        /// <param name="count"></param>
        string process_name(int count);
    }
}
