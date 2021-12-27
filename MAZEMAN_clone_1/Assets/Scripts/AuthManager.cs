using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Firestore;
using TMPro;
using Firebase.Extensions;

public class DisplayName {
    public string name;

    public DisplayName(string _name)
    {
        name = _name;
    }
}

public class AuthManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public FirebaseFirestore fs;

    [Header("Sign In")]
    public TMP_InputField username;
    public TMP_InputField password;

    //public List<DisplayName> displayNames = new List<DisplayName>();  
    Dictionary<string, object> uName;
    public ListenerRegistration listener;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all resolve all firebase dependencies:  " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
    }

    public void LoginButton()
    {
        StartCoroutine(Login(username.text, password.text));
    }

    private IEnumerator Login(string _email, string _password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            //warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            user = LoginTask.Result;
            listener = fs.Collection("users").Document(user.UserId).Listen(snap =>
            {
                var test = snap.ConvertTo<DisplayName>();
                Debug.LogFormat("User signed in successfully: {0} ({1})", test.name, user.Email);

            });

            //fs.Collection("users").Document(user.UserId).GetSnapshotAsync().ContinueWith(task =>
            //{
            //    DocumentSnapshot snap = task.Result;
            //    if (snap.Exists)
            //    {
            //        uName = snap.ToDictionary();
            //        foreach(KeyValuePair<string, object> pair in uName)
            //        {
            //            Debug.LogFormat("User signed in successfully: {0} ({1})", pair.Key, user.Email);
            //        }
            //    }
            //});
            //warningLoginText.text = "";
            //confirmLoginText.text = "Logged In";
        }
    }
}
