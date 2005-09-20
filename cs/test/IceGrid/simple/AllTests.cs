// **********************************************************************
//
// Copyright (c) 2003-2005 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

using System;
using Test;

public class AllTests
{
    private static void
    test(bool b)
    {
        if(!b)
        {
            throw new Exception();
        }
    }

    public static TestIntfPrx allTests(Ice.Communicator communicator)
    {
	Console.Out.Write("testing stringToProxy... ");
	Console.Out.Flush();
	String rf = "test @ TestAdapter";
	Ice.ObjectPrx @base = communicator.stringToProxy(rf);
	test(@base != null);
	Console.Out.WriteLine("ok");

	Console.Out.Write("testing checked cast... ");
	Console.Out.Flush();
	TestIntfPrx obj = TestIntfPrxHelper.checkedCast(@base);
	test(obj != null);
	test(obj.Equals(@base));
	Console.Out.WriteLine("ok");
	
	Console.Out.Write("pinging server... ");
	Console.Out.Flush();
	obj.ice_ping();
	Console.Out.WriteLine("ok");
	
	return obj;
    }

    public static TestIntfPrx
    allTestsWithDeploy(Ice.Communicator communicator)
    {
	Console.Out.Write("testing stringToProxy... ");
	Console.Out.Flush();
	Ice.ObjectPrx @base = communicator.stringToProxy("test @ TestAdapter");
	test(@base != null);
	Ice.ObjectPrx @base2 = communicator.stringToProxy("test");
	test(@base2 != null);
	Console.Out.WriteLine("ok");

	Console.Out.Write("testing checked cast... ");
	Console.Out.Flush();
	TestIntfPrx obj = TestIntfPrxHelper.checkedCast(@base);
	test(obj != null);
	test(obj.Equals(@base));
	TestIntfPrx obj2 = TestIntfPrxHelper.checkedCast(@base2);
	test(obj2 != null);
	test(obj2.Equals(@base2));
	Console.Out.WriteLine("ok");
	
	Console.Out.Write("pinging server... ");
	Console.Out.Flush();
	obj.ice_ping();
	obj2.ice_ping();
	Console.Out.WriteLine("ok");

	Console.Out.Write("testing reference with unknown identity... ");
	Console.Out.Flush();
	try
	{
	    communicator.stringToProxy("unknown/unknown").ice_ping();
	    test(false);
	}
	catch(Ice.NotRegisteredException ex)
	{
	    test(ex.kindOfObject.Equals("object"));
	    test(ex.id.Equals("unknown/unknown"));
	}
	Console.Out.WriteLine("ok");	

	Console.Out.Write("testing reference with unknown adapter... ");
	Console.Out.Flush();
	try
	{
	    communicator.stringToProxy("test @ TestAdapterUnknown").ice_ping();
	    test(false);
	}
	catch(Ice.NotRegisteredException ex)
	{
	    test(ex.kindOfObject.Equals("object adapter"));
	    test(ex.id.Equals("TestAdapterUnknown"));
	}
	Console.Out.WriteLine("ok");	

 	IceGrid.AdminPrx admin = IceGrid.AdminPrxHelper.checkedCast(communicator.stringToProxy("IceGrid/Admin"));
	test(admin != null);

	try
	{
	    admin.setServerActivation("server", IceGrid.ServerActivation.Manual);
	    admin.stopServer("server");
	}
	catch(IceGrid.ServerNotExistException)
	{
	    test(false);
	}
	catch(IceGrid.NodeUnreachableException)
	{
	    test(false);
	}

	Console.Out.Write("testing whether server is still reachable... ");
	Console.Out.Flush();
	try
	{
	    obj = TestIntfPrxHelper.checkedCast(@base);
	    test(false);
	}
	catch(Ice.NoEndpointException)
	{
	}
	try
	{
	    obj2 = TestIntfPrxHelper.checkedCast(@base2);
	    test(false);
	}
	catch(Ice.NoEndpointException)
	{
	}
	
	try
	{
	    admin.setServerActivation("server", IceGrid.ServerActivation.OnDemand);
	}
	catch(IceGrid.ServerNotExistException)
	{
	    test(false);
	}
	catch(IceGrid.NodeUnreachableException)
	{
	    test(false);
	}

	try
	{
	    obj = TestIntfPrxHelper.checkedCast(@base);
	}
	catch(Ice.NoEndpointException)
	{
	    test(false);
	}
	try
	{
	    obj2 = TestIntfPrxHelper.checkedCast(@base2);
	}
	catch(Ice.NoEndpointException)
	{
	    test(false);
	}
	Console.Out.WriteLine("ok");

	return obj;
    }
}
