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
	public partial class PERSON_HEADER : DataStructure {
	
				// Client
	    private string _MANDT;

		// Person Number (Sample Application)
	    private int? _PERSNUMBER;

		// First Name (Sample Application)
	    private string _FIRST_NAME;

		// Last Name (Sample Application)
	    private string _LAST_NAME;

		// Profession (Sample Application)
	    private string _PROFESSION;

		// Gender (Sample Application)
	    private string _SEX;

		// Birthday (Sample Application)
	    private string _BIRTHDAY;

		// Weight (Sample Application)
	    private double? _WEIGHT;

		// Height (Sample Application)
	    private double? _HEIGHT;

		// Category1 (Sample Application)
	    private int? _CATEGORY1;

		// Category2 (Sample Application)
	    private string _CATEGORY2;

		// Created on
	    private string _CREDAT;

		// Created by
	    private string _CRENAM;

		// Create Time
	    private string _CRETIM;

		// Last Changed on
	    private string _CHGDAT;

		// Last Changed by
	    private string _CHGNAM;

		// Last Changed at
	    private string _CHGTIM;
		
		internal static bool IsHeader {
			get {
			    return true;
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

		public string FIRST_NAME { 
			get {
				return _FIRST_NAME;
			}
			set {
				_FIRST_NAME = value;
				RaisePropertyChanged("FIRST_NAME");
			}
		}

		public string LAST_NAME { 
			get {
				return _LAST_NAME;
			}
			set {
				_LAST_NAME = value;
				RaisePropertyChanged("LAST_NAME");
			}
		}

		public string PROFESSION { 
			get {
				return _PROFESSION;
			}
			set {
				_PROFESSION = value;
				RaisePropertyChanged("PROFESSION");
			}
		}

		public string SEX { 
			get {
				return _SEX;
			}
			set {
				_SEX = value;
				RaisePropertyChanged("SEX");
			}
		}

		public string BIRTHDAY { 
			get {
				return _BIRTHDAY;
			}
			set {
				_BIRTHDAY = value;
				RaisePropertyChanged("BIRTHDAY");
			}
		}

		public double? WEIGHT { 
			get {
				return _WEIGHT;
			}
			set {
				_WEIGHT = value;
				RaisePropertyChanged("WEIGHT");
			}
		}

		public double? HEIGHT { 
			get {
				return _HEIGHT;
			}
			set {
				_HEIGHT = value;
				RaisePropertyChanged("HEIGHT");
			}
		}

		public int? CATEGORY1 { 
			get {
				return _CATEGORY1;
			}
			set {
				_CATEGORY1 = value;
				RaisePropertyChanged("CATEGORY1");
			}
		}

		public string CATEGORY2 { 
			get {
				return _CATEGORY2;
			}
			set {
				_CATEGORY2 = value;
				RaisePropertyChanged("CATEGORY2");
			}
		}

		public string CREDAT { 
			get {
				return _CREDAT;
			}
			set {
				_CREDAT = value;
				RaisePropertyChanged("CREDAT");
			}
		}

		public string CRENAM { 
			get {
				return _CRENAM;
			}
			set {
				_CRENAM = value;
				RaisePropertyChanged("CRENAM");
			}
		}

		public string CRETIM { 
			get {
				return _CRETIM;
			}
			set {
				_CRETIM = value;
				RaisePropertyChanged("CRETIM");
			}
		}

		public string CHGDAT { 
			get {
				return _CHGDAT;
			}
			set {
				_CHGDAT = value;
				RaisePropertyChanged("CHGDAT");
			}
		}

		public string CHGNAM { 
			get {
				return _CHGNAM;
			}
			set {
				_CHGNAM = value;
				RaisePropertyChanged("CHGNAM");
			}
		}

		public string CHGTIM { 
			get {
				return _CHGTIM;
			}
			set {
				_CHGTIM = value;
				RaisePropertyChanged("CHGTIM");
			}
		}
	}
}