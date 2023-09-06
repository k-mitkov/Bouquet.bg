import React from "react";
import { beforeEach, vi } from "vitest";

beforeEach(() => {
  (window as any).navigator = mockNavigator;
});

const mockNavigator = {
  geolocation: {
    getCurrentPosition: () => { },
  }
};