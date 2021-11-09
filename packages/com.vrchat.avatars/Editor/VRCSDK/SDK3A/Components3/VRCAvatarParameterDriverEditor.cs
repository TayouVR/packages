#if VRC_SDK_VRCSDK3
using UnityEngine;
using UnityEditor;
using VRC.SDK3.Avatars.Components;
using static VRC.SDKBase.VRC_AvatarParameterDriver;
using Boo.Lang;
using System;
using UnityEditorInternal;
using VRC.SDKBase;
using AnimatorControllerParameterType = UnityEngine.AnimatorControllerParameterType;

[CustomEditor(typeof(VRCAvatarParameterDriver))]
public class AvatarParameterDriverEditor : Editor
{
	VRCAvatarParameterDriver driver;
	string[] parameterNames;
	AnimatorControllerParameterType[] parameterTypes;

	private ReorderableList list;

	enum ChangeTypeBool
	{
		Set = 0,
		Random = 2,
	}

	public ReorderableList List {
		get {
			if (list == null) {
				list = new ReorderableList(driver.parameters, typeof(Parameter));
				list.drawElementCallback += DrawElementCallback;
				list.onAddCallback += delegate(ReorderableList reorderableList) { reorderableList.list.Add(new Parameter() { name = parameterNames.Length > 0 ? parameterNames[0] : "" }); };
				list.elementHeightCallback += ElementHeightCallback;
				list.headerHeight = 1;
			}
			return list;
		}
	}

	private float ElementHeightCallback(int index) {
		float height = EditorGUIUtility.singleLineHeight * 1.25f; // name
		height += EditorGUIUtility.singleLineHeight * 1.25f; // action
		height += EditorGUIUtility.singleLineHeight * 1.25f; // value 1

		Parameter parameter = (Parameter)list.list[index];
		if (parameter.type == ChangeType.Random && (parameterTypes[IndexOf(parameterNames, parameter.name)] == AnimatorControllerParameterType.Int || parameterTypes[IndexOf(parameterNames, parameter.name)] == AnimatorControllerParameterType.Float)) {
			height += EditorGUIUtility.singleLineHeight * 1.25f; // value 2
		}

		if (IndexOf(parameterNames, parameter.name) < 0) {
			height += EditorGUIUtility.singleLineHeight * 1.25f * 2; // help box when parameter is empty (no parameters are present in animator
		}
		
		return height;
	}

	public void OnEnable()
	{
		driver = target as VRCAvatarParameterDriver;

		//Build parameter names
		var controller = GetCurrentController();
		if (controller != null)
		{
			//Standard
			List<string> names = new List<string>();
			List<AnimatorControllerParameterType> types = new List<AnimatorControllerParameterType>();
			foreach (var item in controller.parameters)
			{
				names.Add(item.name);
				types.Add(item.type);
			}
			parameterNames = names.ToArray();
			parameterTypes = types.ToArray();
		}
	}

	private void DrawElementCallback(Rect rect, int i, bool isactive, bool isfocused) {
		var param = driver.parameters[i];
		var index = IndexOf(parameterNames, param.name);
		rect.height = EditorGUIUtility.singleLineHeight;

		//Name
		Rect _rect = new Rect(rect.x, rect.y, rect.width - 100, rect.height);
		EditorGUI.LabelField(_rect, "Name");
		if (parameterNames != null)
		{
			EditorGUI.BeginChangeCheck();
			_rect = new Rect(200, rect.y, rect.width - 300, rect.height);
			index = EditorGUI.Popup(_rect, index, parameterNames);
			if (EditorGUI.EndChangeCheck() && index >= 0)
				param.name = parameterNames[index];
		}
		_rect = new Rect(rect.width - 90, rect.y, 130, rect.height);
		param.name = EditorGUI.TextField(_rect, param.name);
		
		rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
		_rect = new Rect(rect);
		
		//Value
		if (index >= 0)
		{
			var type = parameterTypes[index];
			if (type == AnimatorControllerParameterType.Int)
			{
				//Type
				param.type = (VRCAvatarParameterDriver.ChangeType)EditorGUI.EnumPopup(_rect, "Change Type", param.type);

				rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
				_rect = new Rect(rect);
				
				//Value
				if (param.type == ChangeType.Set)
					param.value = Mathf.Clamp(EditorGUI.IntField(_rect, "Value", (int)param.value), 0, 255);
				else if (param.type == ChangeType.Add)
					param.value = Mathf.Clamp(EditorGUI.IntField(_rect, "Value", (int)param.value), -255, 255);
				else if (param.type == ChangeType.Random)
				{
					param.valueMin = Mathf.Clamp(EditorGUI.IntField(_rect, "Min Value", (int)param.valueMin), 0, 255);
					rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
					_rect = new Rect(rect);
					param.valueMax = Mathf.Clamp(EditorGUI.IntField(_rect, "Max Value", (int)param.valueMax), param.valueMin, 255);
				}
			}
			else if (type == AnimatorControllerParameterType.Float)
			{
				//Type
				param.type = (VRCAvatarParameterDriver.ChangeType)EditorGUI.EnumPopup(_rect, "Change Type", param.type);

				rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
				_rect = new Rect(rect);

				//Value
				if (param.type == ChangeType.Set || param.type == ChangeType.Add)
					param.value = Mathf.Clamp(EditorGUI.FloatField(_rect, "Value", param.value), -1f, 1);
				else if (param.type == ChangeType.Random)
				{
					param.valueMin = Mathf.Clamp(EditorGUI.FloatField(_rect, "Min Value", param.valueMin), -1f, 1);
					rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
					_rect = new Rect(rect);
					param.valueMax = Mathf.Clamp(EditorGUI.FloatField(_rect, "Max Value", param.valueMax), param.valueMin, 1);
				}
			}
			else if (type == AnimatorControllerParameterType.Bool)
			{
				//Type
				param.type = (VRCAvatarParameterDriver.ChangeType)EditorGUI.EnumPopup(_rect, "Change Type", (ChangeTypeBool)param.type);

				rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
				_rect = new Rect(rect);

				//Value
				if (param.type == ChangeType.Set)
					param.value = EditorGUI.Toggle(_rect, "Value", param.value != 0) ? 1f : 0f;
				else
					param.chance = Mathf.Clamp(EditorGUI.FloatField(_rect, "Chance", param.chance), 0f, 1f);
			}
			else if (type == AnimatorControllerParameterType.Trigger)
			{
				//Type
				param.type = (VRCAvatarParameterDriver.ChangeType)EditorGUI.EnumPopup(_rect, "Change Type", (ChangeTypeBool)param.type);

				rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
				_rect = new Rect(rect);

				//Chance
				if (param.type == ChangeType.Random)
					param.chance = Mathf.Clamp(EditorGUI.FloatField(_rect, "Chance", param.chance), 0f, 1f);
			}
		}
		else
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUI.EnumPopup(_rect, "Change Type", param.type);
			if(param.type == ChangeType.Random)
			{
				rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
				_rect = new Rect(rect);
				EditorGUI.FloatField(_rect, "Min Value", param.valueMin);
				rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
				_rect = new Rect(rect);
				EditorGUI.FloatField(_rect, "Max Value", param.valueMax);
			}
			else
			{
				rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
				_rect = new Rect(rect);
				EditorGUI.FloatField(_rect, "Value", param.value);
			}
			EditorGUI.EndDisabledGroup();
			rect.y += EditorGUIUtility.singleLineHeight * 1.25f;
			_rect = new Rect(rect.x - 30, rect.y, rect.width + 30, rect.height * 2);
			DrawInfoBox(_rect, "WARNING: Parameter not found. Make sure you defined it on the animation controller.");
		}
	}

	static UnityEditor.Animations.AnimatorController GetCurrentController()
	{
		UnityEditor.Animations.AnimatorController controller = null;
		var toolType = Type.GetType("UnityEditor.Graphs.AnimatorControllerTool, UnityEditor.Graphs");
		var tool = EditorWindow.GetWindow(toolType);
		var controllerProperty = toolType.GetProperty("animatorController", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
		if (controllerProperty != null)
		{
			controller = controllerProperty.GetValue(tool, null) as UnityEditor.Animations.AnimatorController;
		}
		else
			Debug.LogError("Unable to find animator window.", tool);
		return controller;
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		serializedObject.Update();

		//Info
		EditorGUILayout.HelpBox("This behaviour modifies parameters on this and all other animation controllers referenced on the avatar descriptor.", MessageType.Info);
		EditorGUILayout.HelpBox("You should primarily be driving expression parameters as they are the only variables that sync across the network. Changes to any other parameter will not be synced across the network.", MessageType.Info);

		//Data
		driver.localOnly = EditorGUILayout.Toggle("Local Only", driver.localOnly);

		//Check for info message
		bool usesAddOrRandom = false;
		foreach(var param in driver.parameters)
		{
			if (param.type != ChangeType.Set)
				usesAddOrRandom = true;
		}
		if(usesAddOrRandom && !driver.localOnly)
			EditorGUILayout.HelpBox("Using Add & Random may not produce the same result when run on remote instance of the avatar.  When using these modes it's suggested you use a synced parameter and use the local only option.", MessageType.Warning);

		List.DoLayoutList();

		//End
		serializedObject.ApplyModifiedProperties();
		if (EditorGUI.EndChangeCheck())
			EditorUtility.SetDirty(this);
	}

	private int IndexOf(string[] array, string value)
	{
		if (array == null)
			return -1;
		for(int i=0; i<array.Length; i++)
		{
			if (array[i] == value)
				return i;
		}
		return -1;
	}

	private void DrawInfoBox(Rect rect, string text)
	{
		EditorGUI.indentLevel += 2;
		EditorGUI.LabelField(rect, text, EditorStyles.textArea);
		EditorGUI.indentLevel -= 2;
	}
}
#endif
