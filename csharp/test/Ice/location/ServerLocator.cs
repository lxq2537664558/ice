// **********************************************************************
//
// Copyright (c) 2003-2016 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

using System;
using System.Diagnostics;

public class ServerLocator : Test.TestLocatorDisp_
{
    public ServerLocator(ServerLocatorRegistry registry, Ice.LocatorRegistryPrx registryPrx)
    {
        _registry = registry;
        _registryPrx = registryPrx;
        _requestCount = 0;
    }
    
    public override void
    findAdapterByIdAsync(string adapter, Action<Ice.ObjectPrx> response, Action<Exception> exception,
                         Ice.Current current)
    {
        ++_requestCount;
        if(adapter.Equals("TestAdapter10") || adapter.Equals("TestAdapter10-2"))
        {
            Debug.Assert(current.encoding.Equals(Ice.Util.Encoding_1_0));
            response(_registry.getAdapter("TestAdapter"));
        }
        else
        {
            // We add a small delay to make sure locator request queuing gets tested when
            // running the test on a fast machine
            System.Threading.Thread.Sleep(1);
            response(_registry.getAdapter(adapter));
        }
    }
    
    public override void
    findObjectByIdAsync(Ice.Identity id, Action<Ice.ObjectPrx> response, Action<Exception> exception,
                        Ice.Current current)
    {
        ++_requestCount;
        // We add a small delay to make sure locator request queuing gets tested when
        // running the test on a fast machine
        System.Threading.Thread.Sleep(1);
        response(_registry.getObject(id));
    }
    
    public override Ice.LocatorRegistryPrx getRegistry(Ice.Current current)
    {
        return _registryPrx;
    }

    public override int getRequestCount(Ice.Current current)
    {
        return _requestCount;
    }

    private ServerLocatorRegistry _registry;
    private Ice.LocatorRegistryPrx _registryPrx;
    private int _requestCount;
}
