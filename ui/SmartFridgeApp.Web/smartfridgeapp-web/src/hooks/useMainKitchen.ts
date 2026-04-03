import { useState, useEffect, useSyncExternalStore } from "react";

const STORAGE_KEY = "mainKitchen";
const EVENT_NAME = "mainKitchenChanged";

export interface MainKitchen {
  id: string;
  name: string;
}

function getSnapshot(): MainKitchen | null {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null;
  }
}

function subscribe(callback: () => void): () => void {
  window.addEventListener(EVENT_NAME, callback);
  window.addEventListener("storage", callback);
  return () => {
    window.removeEventListener(EVENT_NAME, callback);
    window.removeEventListener("storage", callback);
  };
}

export function setMainKitchen(id: string, name: string) {
  localStorage.setItem(STORAGE_KEY, JSON.stringify({ id, name }));
  window.dispatchEvent(new Event(EVENT_NAME));
}

export function clearMainKitchen() {
  localStorage.removeItem(STORAGE_KEY);
  window.dispatchEvent(new Event(EVENT_NAME));
}

export function useMainKitchen(): MainKitchen | null {
  return useSyncExternalStore(subscribe, getSnapshot, () => null);
}
