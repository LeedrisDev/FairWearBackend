// JavaScript to handle category creation, deletion, and form submission
document.addEventListener("DOMContentLoaded", function () {
    const categoriesInput = document.getElementById("categories-input");
    const categoriesList = document.getElementById("categories-list");
    const addCategoryButton = document.getElementById("add-category");
    const categories = []; // Array to store created categories

    const rangesInput = document.getElementById("ranges-input");
    const rangesList = document.getElementById("ranges-list");
    const addRangeButton = document.getElementById("add-range");
    const ranges = []; // Array to store created ranges

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
            categoryTextElement.className = "category-text";
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
            categories.push(categoryText);

            console.warn(categories)
        }
    }

    function addRange() {
        const rangeText = rangesInput.value.trim();
        if (rangeText !== "") {
            const rangeElement = document.createElement("div");
            rangeElement.className = "range";

            // Create the colored rectangle element
            const rangeRectangle = document.createElement("div");
            rangeRectangle.className = "range-rectangle";
            rangeElement.appendChild(rangeRectangle);

            // Create the text element
            const rangeTextElement = document.createElement("span");
            rangeTextElement.className = "range-text margin-y-0";
            rangeTextElement.textContent = rangeText;
            rangeRectangle.appendChild(rangeTextElement);

            // Create the delete button
            const deleteButton = document.createElement("button");
            deleteButton.className = "delete-range";
            rangeRectangle.appendChild(deleteButton);
            deleteButton.addEventListener("click", function () {
                // Remove the range element from the DOM
                rangeElement.remove();

                // Remove the range text from the ranges array
                const rangeIndex = ranges.indexOf(rangeText);
                if (rangeIndex !== -1) {
                    ranges.splice(rangeIndex, 1);
                }
            });

            // Add the range to the list
            rangesList.appendChild(rangeElement);
            rangesInput.value = "";
            ranges.push(rangeText);
        }
    }

    // Handle "Add" button click for categories
    addCategoryButton.addEventListener("click", addCategory);

    // Handle "Add" button click for ranges
    addRangeButton.addEventListener("click", addRange);

    // Handle form submission
    document.querySelector("form").addEventListener("submit", function () {
        // Transform the categories and ranges arrays into JSON strings and store them in hidden input fields
        if (categories.length > 0) {
            categoriesInput.value = JSON.stringify(categories);
        }
        
        if (ranges.length > 0) {
            rangesInput.value = JSON.stringify(ranges);
        }
    });
});