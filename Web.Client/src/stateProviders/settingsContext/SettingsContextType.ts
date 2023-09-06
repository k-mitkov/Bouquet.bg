
export type ThemeType = 'system' | 'dark' | 'light';

export interface SettingsContextType {

    theme: ThemeType,
    setApplicationTheme(themeType: ThemeType): void;
}

