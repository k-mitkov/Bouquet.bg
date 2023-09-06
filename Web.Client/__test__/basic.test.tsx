import React from 'react';
import {beforeEach, describe, expect, it, vi } from 'vitest';
import {render, screen} from '@testing-library/react';
import Login from '../src/components/Login';
import App from '../src/App'
import { MemoryRouter } from 'react-router-dom';

//mockReactRouter();

describe("App", () => {
    it("App", () => {
      render(<MemoryRouter><App /></MemoryRouter>);
      screen.debug();
    });
  });