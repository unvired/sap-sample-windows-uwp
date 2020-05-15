//	Generated using Unvired Modeller - Build R-4.000.0129

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Unvired.Kernel.SQLite;
using Unvired.Kernel.Model;
using Unvired.Kernel.UWP.Model;

namespace Entity {
	// This class is part of the BE "PERSON". 
	public partial class E_MAIL : DataStructure {
	
				//Attribute to retrieve the name of the Header in a static manner. This is used internally for SQlite logic.
		internal byte MyHeader_PERSON_HEADER {get; set;}

		// Client
	    private string _MANDT;

		// Person Number (Sample Application)
	    private int? _PERSNUMBER;

		// Seqeunce Number (Sample Application)
	    private int? _SEQNO_E_MAIL;

		// E-mail Address (Sample Application)
	    private string _E_ADDR;

		// E-mail Address Description (Sample Application)
	    private string _E_ADDR_TEXT;
		
		internal static bool IsHeader {
			get {
			    return false;
			}
	    }
			
		[Unique]
		public string MANDT { 
			get {
				return _MANDT;
			}
			set {
				_MANDT = value;
				RaisePropertyChanged("MANDT");
			}
		}

		[Unique]
		public int? PERSNUMBER { 
			get {
				return _PERSNUMBER;
			}
			set {
				_PERSNUMBER = value;
				RaisePropertyChanged("PERSNUMBER");
			}
		}

		[Unique]
		public int? SEQNO_E_MAIL { 
			get {
				return _SEQNO_E_MAIL;
			}
			set {
				_SEQNO_E_MAIL = value;
				RaisePropertyChanged("SEQNO_E_MAIL");
			}
		}

		public string E_ADDR { 
			get {
				return _E_ADDR;
			}
			set {
				_E_ADDR = value;
				RaisePropertyChanged("E_ADDR");
			}
		}

		public string E_ADDR_TEXT { 
			get {
				return _E_ADDR_TEXT;
			}
			set {
				_E_ADDR_TEXT = value;
				RaisePropertyChanged("E_ADDR_TEXT");
			}
		}
	}
}