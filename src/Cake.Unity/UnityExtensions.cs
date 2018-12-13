﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Unity.Platforms;

namespace Cake.Unity
{
    [CakeAliasCategory("Unity")]
    public static class UnityExtensions
    {
        [CakeMethodAlias]
        [CakeAliasCategory("Build")]
        [CakeNamespaceImport("Cake.Unity.Platforms")]
        public static void UnityBuild(this ICakeContext context, DirectoryPath projectPath, UnityPlatform platform)
        {
            var tool = new UnityRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            tool.Run(context, projectPath, platform);
        }

        [CakeMethodAlias]
        [CakeAliasCategory("Build")]
        public static UnityEditorDescriptor FindUnityEditor(this ICakeContext context, int year) =>
            Enumerable.FirstOrDefault
            (
                from editor in context.FindUnityEditors()
                let version = editor.Version
                where
                    version.Year == year
                orderby
                    version.Stream descending,
                    version.Update descending
                select editor
            );

        [CakeMethodAlias]
        [CakeAliasCategory("Build")]
        public static UnityEditorDescriptor FindUnityEditor(this ICakeContext context, int year, int stream) =>
            Enumerable.FirstOrDefault
            (
                from editor in context.FindUnityEditors()
                let version = editor.Version
                where
                    version.Year == year &&
                    version.Stream == stream
                orderby
                    version.Update descending
                select editor
            );

        [CakeMethodAlias]
        [CakeAliasCategory("Build")]
        public static IReadOnlyCollection<UnityEditorDescriptor> FindUnityEditors(this ICakeContext context) =>
            new SeekerOfEditors(context.Environment, context.Globber, context.Log)
                .Seek();
    }
}
