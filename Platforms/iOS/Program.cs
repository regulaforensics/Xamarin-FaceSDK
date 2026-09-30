using UIKit;

namespace FaceSample;

public class Program
{
	// This is the main entry point of the application.
	static void Main(string[] args)
	{

        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        // Load the transitive binding before native callbacks resolve Common types.
        GC.KeepAlive(typeof(RegulaCommon.iOS.RGLCBaseViewController).Assembly);
        UIApplication.Main(args, null, typeof(AppDelegate));
	}
}

