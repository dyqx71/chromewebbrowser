//
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
//
namespace Cef3
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Cef3.Interop;
    
    // Role: PROXY
    public sealed unsafe partial class CefDomEvent : IDisposable
    {
        internal static CefDomEvent FromNative(cef_domevent_t* ptr)
        {
            return new CefDomEvent(ptr);
        }
        
        internal static CefDomEvent FromNativeOrNull(cef_domevent_t* ptr)
        {
            if (ptr == null) return null;
            return new CefDomEvent(ptr);
        }
        
        private cef_domevent_t* _self;
        
        private CefDomEvent(cef_domevent_t* ptr)
        {
            if (ptr == null) throw new ArgumentNullException("ptr");
            _self = ptr;
        }
        
        ~CefDomEvent()
        {
            if (_self != null)
            {
                Release();
                _self = null;
            }
        }
        
        public void Dispose()
        {
            if (_self != null)
            {
                Release();
                _self = null;
            }
            GC.SuppressFinalize(this);
        }
        
        internal int AddRef()
        {
            return cef_domevent_t.add_ref(_self);
        }
        
        internal int Release()
        {
            return cef_domevent_t.release(_self);
        }
        
        internal int RefCt
        {
            get { return cef_domevent_t.get_refct(_self); }
        }
        
        internal cef_domevent_t* ToNative()
        {
            AddRef();
            return _self;
        }
    }
}
