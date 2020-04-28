namespace BlobbInvasion.Gameplay
{
    public class MainFactory
    {

        //###############
        //##  MEMBERS  ##
        //###############

        private static MainFactory mInstance;

        




        private MainFactory() { }

        //#################
        //##  AUXILIARY  ##
        //#################

        public static MainFactory Instance
        {
            get
            {
                if (mInstance == null) mInstance = new MainFactory();
                return mInstance;
            }
        }


    }
}