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
    
    // Role: HANDLER
    public abstract unsafe partial class CefCookieVisitor
    {
        private static Dictionary<IntPtr, CefCookieVisitor> _roots = new Dictionary<IntPtr, CefCookieVisitor>();
        
        private int _refct;
        private cef_cookie_visitor_t* _self;
        
        protected object SyncRoot { get { return this; } }
        
        private cef_cookie_visitor_t.add_ref_delegate _ds0;
        private cef_cookie_visitor_t.release_delegate _ds1;
        private cef_cookie_visitor_t.get_refct_delegate _ds2;
        private cef_cookie_visitor_t.visit_delegate _ds3;
        
        protected CefCookieVisitor()
        {
            _self = cef_cookie_visitor_t.Alloc();
        
            _ds0 = new cef_cookie_visitor_t.add_ref_delegate(add_ref);
            _self->_base._add_ref = Marshal.GetFunctionPointerForDelegate(_ds0);
            _ds1 = new cef_cookie_visitor_t.release_delegate(release);
            _self->_base._release = Marshal.GetFunctionPointerForDelegate(_ds1);
            _ds2 = new cef_cookie_visitor_t.get_refct_delegate(get_refct);
            _self->_base._get_refct = Marshal.GetFunctionPointerForDelegate(_ds2);
            _ds3 = new cef_cookie_visitor_t.visit_delegate(visit);
            _self->_visit = Marshal.GetFunctionPointerForDelegate(_ds3);
        }
        
        ~CefCookieVisitor()
        {
            Dispose(false);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_self != null)
            {
                cef_cookie_visitor_t.Free(_self);
                _self = null;
            }
        }
        
        private int add_ref(cef_cookie_visitor_t* self)
        {
            lock (SyncRoot)
            {
                var result = ++_refct;
                if (result == 1)
                {
                    lock (_roots) { _roots.Add((IntPtr)_self, this); }
                }
                return result;
            }
        }
        
        private int release(cef_cookie_visitor_t* self)
        {
            lock (SyncRoot)
            {
                var result = --_refct;
                if (result == 0)
                {
                    lock (_roots) { _roots.Remove((IntPtr)_self); }
                }
                return result;
            }
        }
        
        private int get_refct(cef_cookie_visitor_t* self)
        {
            return _refct;
        }
        
        internal cef_cookie_visitor_t* ToNative()
        {
            add_ref(_self);
            return _self;
        }
        
        [Conditional("DEBUG")]
        private void CheckSelf(cef_cookie_visitor_t* self)
        {
            if (_self != self) throw ExceptionBuilder.InvalidSelfReference();
        }
        
    }
}
