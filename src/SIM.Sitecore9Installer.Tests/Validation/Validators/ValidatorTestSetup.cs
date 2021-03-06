﻿//provide test setup common for all validators.
using AutoFixture;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using Task = SIM.Sitecore9Installer.Tasks.Task;

namespace SIM.Sitecore9Installer.Tests.Validation.Validators
{
  public class ValidatorTestSetup: IEnumerable<object[]>
  {
    static Fixture _fix;
    public static Fixture Fix
    {
      get
      {
        if (_fix == null)
        {
          _fix = new Fixture();
          _fix.Register<Tasks.Task>(() =>
          {
            Task t = Substitute.For<Task>(_fix.Create<string>(), _fix.Create<int>(), null, new List<InstallParam>(), new Dictionary<string, string>());
            t.GlobalParams.Returns(new List<InstallParam>());
            t.LocalParams.Returns(new List<InstallParam>());
            return t;
          });
        }

        return _fix;
      }
    }

    public IEnumerator<object[]> GetEnumerator()
    {
      yield return new object[] { Fix.CreateMany<Task>(2) };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public static Task CreateTask(string taskName, string[] paramNames, string[] paramValues)
    {
      var task = Substitute.For<Task>("someTask", 1, null, new List<InstallParam>(),
        new Dictionary<string, string>());
      List<InstallParam> iParams = new List<InstallParam>();
      for (int i = 0; i < paramNames.Length; ++i)
      {
        InstallParam p = new InstallParam(paramNames[i], paramValues[i]);
        iParams.Add(p);
      }

      task.LocalParams.Returns(iParams);
      return task;
    }
  }
}
