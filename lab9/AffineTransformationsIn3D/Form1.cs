﻿using System.Windows.Forms;
using System;
using AffineTransformationsIn3D.Geometry;

namespace AffineTransformationsIn3D
{
    public partial class Form1 : Form
    {
        private IDrawable CurrentDrawable
        {
            get
            {
                return sceneView4.Drawable;
            }
            set
            {
                sceneView1.Drawable = value;
                sceneView2.Drawable = value;
                sceneView3.Drawable = value;
                sceneView4.Drawable = value;
                RefreshScenes();
            }
        }

        private Camera camera;

        public Form1()
        {
            InitializeComponent();
            CurrentDrawable = Models.Cube(0.5);
            sceneView1.Camera = new Camera(new Vector(0, 0, 0), 0, 0, 
                Transformations.OrthogonalProjection());
            sceneView2.Camera = new Camera(new Vector(0, 0, 0), 0, 0,
                Transformations.RotateY(-Math.PI / 2) 
                * Transformations.OrthogonalProjection());
            sceneView3.Camera = new Camera(new Vector(0, 0, 0), 0, 0,
                Transformations.RotateX(Math.PI / 2)
                * Transformations.OrthogonalProjection());
            Matrix projection = Transformations.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.16, 20);
            camera = new Camera(new Vector(1, 1, 1), Math.PI / 4, -Math.Atan(1 / Math.Sqrt(3)), projection);
            sceneView4.Camera = camera;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees / 180 * Math.PI;
        }

        private void RefreshScenes()
        {
            sceneView1.Refresh();
            sceneView2.Refresh();
            sceneView3.Refresh();
            sceneView4.Refresh();
        }

        private void Scale(object sender, EventArgs e)
        {
            double scalingX = (double)numericUpDown1.Value;
            double scalingY = (double)numericUpDown2.Value;
            double scalingZ = (double)numericUpDown3.Value;
            CurrentDrawable.Apply(Transformations.Scale(scalingX, scalingY, scalingZ));
            RefreshScenes();
        }

        private void Rotate(object sender, EventArgs e)
        {
            double rotatingX = DegreesToRadians((double)numericUpDown4.Value);
            double rotatingY = DegreesToRadians((double)numericUpDown5.Value);
            double rotatingZ = DegreesToRadians((double)numericUpDown6.Value);
            CurrentDrawable.Apply(Transformations.RotateX(rotatingX)
                * Transformations.RotateY(rotatingY)
                * Transformations.RotateZ(rotatingZ));
            RefreshScenes();
        }

        private void Translate(object sender, EventArgs e)
        {
            double translatingX = (double)numericUpDown7.Value;
            double translatingY = (double)numericUpDown8.Value;
            double translatingZ = (double)numericUpDown9.Value;
            CurrentDrawable.Apply(Transformations.Translate(translatingX, translatingY, translatingZ));
            RefreshScenes();
        }

        private void Reflect(object sender, EventArgs e)
        {
            Matrix reflection;
            if (radioButton1.Checked)
                reflection = Transformations.ReflectX();
            else if (radioButton2.Checked)
                reflection = Transformations.ReflectY();
            else if (radioButton3.Checked)
                reflection = Transformations.ReflectZ();
            else throw new Exception("Unreachable statement");
            CurrentDrawable.Apply(reflection);
            RefreshScenes();
        }

        private void RotateAroundCenter(object sender, EventArgs e)
        {
            double angleX = DegreesToRadians((double)numericUpDown10.Value);
            double angleY = DegreesToRadians((double)numericUpDown11.Value);
            double angleZ = DegreesToRadians((double)numericUpDown12.Value);
            var p = CurrentDrawable.Center;
            CurrentDrawable.Apply(Transformations.RotateAroundPoint(p, angleX, angleY, angleZ));
            RefreshScenes();
        }

        private void RotateAroundLine(object sender, EventArgs e)
        {
            Vector a = new Vector(
                (double)numericUpDownPoint1X.Value, 
                (double)numericUpDownPoint1Y.Value, 
                (double)numericUpDownPoint1Z.Value);
            Vector b = new Vector(
                (double)numericUpDownPoint2X.Value, 
                (double)numericUpDownPoint2Y.Value, 
                (double)numericUpDownPoint2Z.Value);
            var angle = DegreesToRadians((double)numericUpDownAngle.Value);
            CurrentDrawable.Apply(Transformations.RotateAroundLine(a, b, angle));
            RefreshScenes();
        }

        private void ChangeModel(object sender, EventArgs e)
        {
            var dialog = new FormChangeModel(sceneView4.Camera);
            if (DialogResult.OK != dialog.ShowDialog()) return;
            if (null == dialog.SelectedModel) return;
            CurrentDrawable = dialog.SelectedModel;
        }

        private void SaveFile(object sender, EventArgs e)
        {
            if (!(CurrentDrawable is Mesh)) return;
            var mesh = (Mesh)CurrentDrawable;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    mesh.Save(saveDialog.FileName);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно сохранить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadFile(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                CurrentDrawable = new Mesh(openDialog.FileName);
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении файла",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            double delta = 0.3;
            switch (keyData)
            {
                case Keys.W: camera.Position *= Transformations.Translate(0.1 * camera.Forward); break;
                case Keys.A: camera.Position *= Transformations.Translate(0.1 * camera.Left); break;
                case Keys.S: camera.Position *= Transformations.Translate(0.1 * camera.Backward); break; 
                case Keys.D: camera.Position *= Transformations.Translate(0.1 * camera.Right); break;
                case Keys.Left: camera.AngleY += delta; break;
                case Keys.Right: camera.AngleY -= delta; break;
                case Keys.Up: camera.AngleX += delta; break;
                case Keys.Down: camera.AngleX -= delta; break;
            }
            RefreshScenes();
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
