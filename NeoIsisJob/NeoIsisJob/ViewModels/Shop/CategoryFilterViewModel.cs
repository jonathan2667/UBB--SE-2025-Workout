// <copyright file="CategoryFilterViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Workout.Core.Models;

namespace NeoIsisJob.ViewModels.Shop
{
    using NeoIsisJob.Proxy;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;



    /// <summary>
    /// ViewModel for managing and filtering product categories.
    /// </summary>
    public class CategoryFilterViewModel : INotifyPropertyChanged
    {
        private readonly CategoryServiceProxy categoryServiceProxy;

        private CategoryModel? selectedCategory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryFilterViewModel"/> class.
        /// </summary>
        /// <param name="categoryService">The category service used to load categories.</param>
        public CategoryFilterViewModel()
        {
            this.categoryServiceProxy = new CategoryServiceProxy();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the list of available categories.
        /// </summary>
        public ObservableCollection<CategoryModel> Categories { get; } = new ();

        /// <summary>
        /// Gets or sets the selected category.
        /// </summary>
        public CategoryModel? SelectedCategory
        {
            get => this.selectedCategory;
            set
            {
                if (this.selectedCategory != value)
                {
                    this.selectedCategory = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Loads categories asynchronously and populates the Categories collection.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LoadCategoriesAsync()
        {
            var categories = await this.categoryServiceProxy.GetAllAsync();

            this.Categories.Clear();
            foreach (var category in categories)
            {
                this.Categories.Add(category);
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
