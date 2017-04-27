﻿/* This code is licensed under the NullSpace Developer Agreement, available here:
** ***********************
** http://www.hardlightvr.com/wp-content/uploads/2017/01/NullSpace-SDK-License-Rev-3-Jan-2016-2.pdf
** ***********************
** Make sure that you have read, understood, and agreed to the Agreement before using the SDK
*/

using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NullSpace.SDK.FileUtilities;

namespace NullSpace.SDK.Demos
{
	public class PackageViewer : MonoBehaviour
	{
		public Text Folder;
		public PopulateContainer fileContainer;
		public Button commander;
		public string path;
		public string myName = "";
		public string myNameSpace = "";
		public TestHaptics testHaptics;

		private AssetTool _assetTool;
		//When a directory is 'opened'
		public void Init(AssetTool tool, AssetTool.PackageInfo package)
		{
			_assetTool = tool;
			myName = Path.GetFileName(package.path);
			myNameSpace = package.@namespace;
			Folder.text = myName + " Contents";

			path = package.path;

			PopulateMyDirectory(path);
		}
		private List<string> retrieveFilesInFolder(string folderPath)
		{
			
			string[] unfilteredFiles = Directory.GetFiles(folderPath);
			
			return unfilteredFiles.Where((string filename) =>
			{
				string ext = Path.GetExtension(filename);
				return ext == ".pattern"
					|| ext == ".sequence"
					|| ext == ".experience";

			}).ToList();
		}
		//Fill the directory with library elements based on the haptics found
		void PopulateMyDirectory(string path)
		{
			var validSequences = retrieveFilesInFolder(path + "/sequences");
			var validPatterns = retrieveFilesInFolder(path + "/patterns");
			var validExperiences = retrieveFilesInFolder(path + "/experiences");

			var allFiles = validSequences.Concat(validPatterns).Concat(validExperiences);
		
			//A natural result of the haptics being loaded by order of folder means they'll be pre-sorted.
			foreach (string element in allFiles)
			{
				CreateRepresentations(element);
			}

			//Make sure we scroll to the top of the ScrollRect
			ScrollRect sRect = GetComponentInChildren<ScrollRect>();
			sRect.verticalNormalizedPosition = 1;
		}

		//This returns a bool to prep for future failure possibilities, such as initialize/validation failure.
		public bool CreateRepresentations(string element)
		{
			//Debug.Log(s + "\n");
			LibraryElement libEle = fileContainer.AddPrefabToContainerReturn().GetComponent<LibraryElement>();
			libEle.playButton.transform.localScale = Vector3.one;
			libEle.playButton.name = element;

			//Elements need to be initialized so they get the proper name/icon/color
			libEle.Init(_assetTool, element, myNameSpace);

			return true;
		}

		public bool SortElements()
		{
			//Sort the elements in the specified order.
			//TODO: Add element sorting.
			return true;
		}
	}
}