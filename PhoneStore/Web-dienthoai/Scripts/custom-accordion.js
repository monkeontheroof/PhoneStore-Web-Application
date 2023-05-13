const accordionContent = document.querySelector(".accordion-content");

const arr = Array.from(accordionContent);

arr.forEach((item, index) => {
    let header = item.querySelector("header");
    header.addEventListener("click", () => {
        item.classList.toggle("open");

        let description = item.querySelector(".description");
        if (item.classList.contains(open)) {
            description.style.height = `${description.scrollHeight}px`;
        } else {
            description.style.height = "0px";
        }
        console.log(description);
    })
})