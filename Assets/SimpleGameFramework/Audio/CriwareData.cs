namespace AudioFramework
{
    public class CriwareData : IAudioData
    {
        public string cueSheet;
        public string acbPath;
        public string awbPath;
        public string cueName;

        public CriwareData (string cueSheet, string cueName, string acbPath, string awbPath)
        {
            this.cueSheet = cueSheet;
            this.acbPath  = acbPath;
            this.awbPath  = awbPath;
            this.cueName  = cueName;
        }
    }
}