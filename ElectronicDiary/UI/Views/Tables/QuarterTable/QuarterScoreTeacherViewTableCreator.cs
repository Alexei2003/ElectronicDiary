using ElectronicDiary.UI.Views.Tables.BaseTable;

namespace ElectronicDiary.UI.Views.Tables.QuarterTable
{
    public class QuarterScoreTeacherViewTableCreator : BaseViewTableCreator<long, long>
    {
        public QuarterScoreTeacherViewTableCreator()
        {
            _attendance = false;
        }
    }
}
