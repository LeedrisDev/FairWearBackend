// JavaScript to handle category creation, deletion, and form submission
document.addEventListener("DOMContentLoaded", function () {
    const categoriesInput = document.getElementById("categories-input");
    const categoriesList = document.getElementById("categories-list");
    const addCategoryButton = document.getElementById("add-category");
    const categories = []; // Array to store created categories

    function addCategory() {
        const categoryText = categoriesInput.value.trim();
        if (categoryText !== "") {
            const categoryElement = document.createElement("div");
            categoryElement.className = "category";

            // Create the colored rectangle element
            const categoryRectangle = document.createElement("div");
            categoryRectangle.className = "category-rectangle";
            categoryElement.appendChild(categoryRectangle);

            // Create the text element
            const categoryTextElement = document.createElement("span");
            categoryTextElement.className = "category-text margin-y-0";
            categoryTextElement.textContent = categoryText;
            categoryRectangle.appendChild(categoryTextElement);

            // Create the delete button
            const deleteButton = document.createElement("button");
            deleteButton.className = "delete-category";
            categoryRectangle.appendChild(deleteButton);
            deleteButton.addEventListener("click", function () {
                // Remove the category element from the DOM
                categoryElement.remove();

                // Remove the category text from the categories array
                const categoryIndex = categories.indexOf(categoryText);
                if (categoryIndex !== -1) {
                    categories.splice(categoryIndex, 1);
                }
            });

            // Add the ticket to the list
            categoriesList.appendChild(categoryElement);
            categoriesInput.value = "";
        }
    }

    // Handle "Add" button click
    addCategoryButton.addEventListener("click", addCategory);

    // Handle form submission
    document.querySelector("form").addEventListener("submit", function () {
        // Transform the categories array into a JSON string and store it in a hidden input field
        const categoriesInput = document.createElement("input");
        categoriesInput.type = "hidden";
        categoriesInput.name = "BrandEntity.Categories";
        categoriesInput.value = JSON.stringify(categories);
        this.appendChild(categoriesInput); // Append the hidden input to the form
    });
});