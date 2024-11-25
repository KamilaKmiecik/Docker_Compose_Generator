document.getElementById('add-service').addEventListener('click', function () {
    const container = document.getElementById('services-container');
    const index = container.children.length;

    fetch(`/DockerCompose/GetServicePartial?index=${index}`)
        .then(response => response.text())
        .then(html => {
            const div = document.createElement('div');
            div.innerHTML = html;
            container.appendChild(div);

            div.scrollIntoView({ behavior: 'smooth', block: 'start' });
        });
});

document.getElementById('add-network').addEventListener('click', function () {
    const container = document.getElementById('networks-container');
    const index = container.children.length;

    fetch(`/DockerCompose/GetNetworkPartial?index=${index}`)
        .then(response => response.text())
        .then(html => {
            const div = document.createElement('div');
            div.innerHTML = html;
            container.appendChild(div);

            // Auto-scroll
            div.scrollIntoView({ behavior: 'smooth', block: 'start' });
        })
        .catch(error => console.error('Error adding network:', error));
});


document.getElementById('add-volume').addEventListener('click', function () {
    const container = document.getElementById('volumes-container');
    const index = container.children.length;

    fetch(`/DockerCompose/GetVolumePartial?index=${index}`)
        .then(response => response.text())
        .then(html => {
            const div = document.createElement('div');
            div.innerHTML = html;
            container.appendChild(div);

            div.scrollIntoView({ behavior: 'smooth', block: 'start' });
        });

});



//document.getElementById('add-service').addEventListener('click', function () {
//    addDynamicElement('services-container', '/DockerCompose/GetServicePartial', 'Service');
//});

//document.getElementById('add-network').addEventListener('click', function () {
//    addDynamicElement('networks-container', '/DockerCompose/GetNetworkPartial', 'Network');
//});

//document.getElementById('add-volume').addEventListener('click', function () {
//    addDynamicElement('volumes-container', '/DockerCompose/GetVolumePartial', 'Volume');
//});

function addDynamicElement(containerId, endpoint, type) {
    const container = document.getElementById(containerId);
    const index = container.children.length;

    fetch(`${endpoint}?index=${index}`)
        .then(response => response.text())
        .then(html => {
            const div = document.createElement('div');
            div.classList.add(`${type.toLowerCase()}-item`);
            div.innerHTML = html;

            const removeButton = document.createElement('button');
            removeButton.textContent = `Remove ${type}`;
            removeButton.classList.add('remove-button');
            removeButton.type = 'button';
            removeButton.addEventListener('click', function () {
                container.removeChild(div);
            });

            div.appendChild(removeButton);
            container.appendChild(div);
        })
        .catch(error => console.error(`Error adding ${type}:`, error));

}

document.addEventListener('click', function (event) {
    const target = event.target;

    if (target.classList.contains('add-ipam-button')) {
        const index = target.getAttribute('data-network-index');
        const container = document.getElementById(`ipam-configurations-${index}`);
        const ipamIndex = container.children.length;

        const ipamHtml = `
            <div class="ipam-item">
                <label>Subnet:</label>
                <input type="text" name="Networks[${index}].Ipam.Configurations[${ipamIndex}].Subnet" />

                <label>Gateway:</label>
                <input type="text" name="Networks[${index}].Ipam.Configurations[${ipamIndex}].Gateway" />

                <button type="button" class="remove-ipam-button">Remove IPAM</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', ipamHtml);
    }

    if (target.classList.contains('remove-ipam-button')) {
        target.closest('.ipam-item').remove();
    }

    if (target.classList.contains('add-driver-option-button')) {
        const index = target.getAttribute('data-network-index');
        const container = document.getElementById(`driver-options-${index}`);
        const driverOptionHtml = `
            <div class="driver-option-item">
                <label>Key:</label>
                <input type="text" name="Networks[${index}].DriverOptions[newKey]" />

                <label>Value:</label>
                <input type="text" name="Networks[${index}].DriverOptions[newValue]" />

                <button type="button" class="remove-driver-option-button">Remove Option</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', driverOptionHtml);
    }

    if (target.classList.contains('remove-driver-option-button')) {
        target.closest('.driver-option-item').remove();
    }
});


document.addEventListener('click', function (event) {
    const target = event.target;

    if (target.classList.contains('add-env-button')) {
        const index = target.getAttribute('data-service-index');
        const container = document.getElementById(`env-container-${index}`);
        const envIndex = container.children.length;

        const envHtml = `
            <div class="env-item">
                <label>Key:</label>
                <input type="text" name="Services[${index}].Environment[${envIndex}].Key" placeholder="Key" />
                <label>Value:</label>
                <input type="text" name="Services[${index}].Environment[${envIndex}].Value" placeholder="Value" />
                <button type="button" class="remove-env-button">Remove</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', envHtml);
    }

    if (target.classList.contains('remove-env-button')) {
        target.closest('.env-item').remove();
    }

    // Ports
    if (target.classList.contains('add-port-button')) {
        const index = target.getAttribute('data-service-index');
        const container = document.getElementById(`port-container-${index}`);
        const portIndex = container.children.length;

        const portHtml = `
            <div class="port-item">
                <label>Host Port:</label>
                <input type="number" name="Services[${index}].Ports[${portIndex}].HostPort" placeholder="Host Port" />
                <label>Container Port:</label>
                <input type="number" name="Services[${index}].Ports[${portIndex}].ContainerPort" placeholder="Container Port" />
                <label>Protocol:</label>
                <select name="Services[${index}].Ports[${portIndex}].Protocol">
                    <option value="tcp">TCP</option>
                    <option value="udp">UDP</option>
                </select>
                <button type="button" class="remove-port-button">Remove</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', portHtml);
    }

    if (target.classList.contains('remove-port-button')) {
        target.closest('.port-item').remove();
    }

    // Volumes
    if (target.classList.contains('add-volume-button')) {
        const index = target.getAttribute('data-service-index');
        const container = document.getElementById(`volume-container-${index}`);
        const volumeIndex = container.children.length;

        const volumeHtml = `
            <div class="volume-item">
                <label>Source:</label>
                <input type="text" name="Services[${index}].Volumes[${volumeIndex}].Source" placeholder="Source" />
                <label>Target:</label>
                <input type="text" name="Services[${index}].Volumes[${volumeIndex}].Target" placeholder="Target" />
                <button type="button" class="remove-volume-button">Remove</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', volumeHtml);
    }

    if (target.classList.contains('remove-volume-button')) {
        target.closest('.volume-item').remove();
    }
});

document.addEventListener('click', function (event) {
    const target = event.target;

    // Driver Options
    if (target.classList.contains('add-driver-option-button')) {
        const index = target.getAttribute('data-volume-index');
        const container = document.getElementById(`driver-options-container-${index}`);
        const driverOptionHtml = `
            <div class="driver-option-item">
                <label>Key:</label>
                <input type="text" name="Volumes[${index}].DriverOptions[newKey]" placeholder="Enter key" />
                <label>Value:</label>
                <input type="text" name="Volumes[${index}].DriverOptions[newValue]" placeholder="Enter value" />
                <button type="button" class="remove-driver-option-button">Remove Option</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', driverOptionHtml);
    }

    if (target.classList.contains('remove-driver-option-button')) {
        target.closest('.driver-option-item').remove();
    }

    // Labels
    if (target.classList.contains('add-label-button')) {
        const index = target.getAttribute('data-volume-index');
        const container = document.getElementById(`labels-container-${index}`);
        const labelHtml = `
            <div class="label-item">
                <label>Key:</label>
                <input type="text" name="Volumes[${index}].Labels[newKey]" placeholder="Enter key" />
                <label>Value:</label>
                <input type="text" name="Volumes[${index}].Labels[newValue]" placeholder="Enter value" />
                <button type="button" class="remove-label-button">Remove Label</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', labelHtml);
    }

    if (target.classList.contains('remove-label-button')) {
        target.closest('.label-item').remove();
    }
});


document.addEventListener('click', function (event) {
    const target = event.target;

    if (target.classList.contains('add-driver-option-button')) {
        const index = target.getAttribute('data-volume-index');
        const container = document.getElementById(`driver-options-container-${index}`);
        const driverOptionHtml = `
            <div class="driver-option-item">
                <label>Key:</label>
                <input type="text" name="Volumes[${index}].DriverOptions[newKey]" placeholder="Enter key" />
                <label>Value:</label>
                <input type="text" name="Volumes[${index}].DriverOptions[newValue]" placeholder="Enter value" />
                <button type="button" class="remove-driver-option-button">Remove Option</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', driverOptionHtml);
    }

    if (target.classList.contains('remove-driver-option-button')) {
        target.closest('.driver-option-item').remove();
    }

    if (target.classList.contains('add-label-button')) {
        const index = target.getAttribute('data-volume-index');
        const container = document.getElementById(`labels-container-${index}`);
        const labelHtml = `
            <div class="label-item">
                <label>Key:</label>
                <input type="text" name="Volumes[${index}].Labels[newKey]" placeholder="Enter key" />
                <label>Value:</label>
                <input type="text" name="Volumes[${index}].Labels[newValue]" placeholder="Enter value" />
                <button type="button" class="remove-label-button">Remove Label</button>
            </div>
        `;
        container.insertAdjacentHTML('beforeend', labelHtml);
    }

    if (target.classList.contains('remove-label-button')) {
        target.closest('.label-item').remove();
    }
});





/////
function updateIndices(containerId, itemPrefix) {
    const container = document.getElementById(containerId);
    const items = container.children;

    for (let i = 0; i < items.length; i++) {
        const item = items[i];
        item.id = `${itemPrefix}-item-${i}`;
        const inputElements = item.querySelectorAll("input, select, textarea");
        inputElements.forEach(input => {
            const name = input.name;
            input.name = name.replace(/\[\d+\]/, `[${i}]`);
        });
    }
}
////
function updateIndices(containerId, itemPrefix) {
    const container = document.getElementById(containerId);
    const items = container.children;

    for (let i = 0; i < items.length; i++) {
        const item = items[i];
        item.id = `${itemPrefix}-item-${i}`;
        const inputElements = item.querySelectorAll("input, select, textarea");
        inputElements.forEach(input => {
            const name = input.name;
            input.name = name.replace(/\[\d+\]/, `[${i}]`);
        });
    }
}

function removeService(index) {
    var id = document.getElementById(`service-item-${index}`)
    const item = id;
    if (item) {
        item.remove(); // Usuń element DOM
        updateIndices("services-container", "service"); // Zaktualizuj indeksy
    }
}

function removeNetwork(index) {
    var id = document.getElementById(`network-item-${index}`);
    const item = id;
    if (item) {
        item.remove(); // Usuń element DOM
        updateIndices("networks-container", "network"); // Zaktualizuj indeksy
    }
}
