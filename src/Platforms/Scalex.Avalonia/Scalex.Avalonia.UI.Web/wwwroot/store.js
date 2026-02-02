export function setLocalStorageItem(name, value) {
	window.localStorage.setItem(name, value);
}

export function getLocalStorageItem(name) {
	return window.localStorage.getItem(name) || '{}';
};