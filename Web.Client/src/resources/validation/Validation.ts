export const errorMessages = {
  required: 'This field is required',
  usernameMinLenght: 'Username must be at least 3 characters long',
  usernameMaxLenght: 'Username must not exceed 20 characters',
  emailMaxLenght: 'Email must not exceed 50 characters',
  invalidEmail: 'Invalid email',
  passwordMinLenght: 'Password must be at least 6 characters long',
  passwordMaxLenght: 'Password must not exceed 20 characters',
  passwordDontMatch: 'Passwords do not match',
  powerMinLength: 'Power must be a positive number',
  priceMinLength: 'Price must be a positive number',
  parkingPriceMinLength: 'Parking price must be a positive number',
  initialFeeMinLength: 'Initial fee must be a positive number',
  objectRequired: 'Object must be selected',
  typeRequired: 'Type must be selected',
  modelRequired: 'Model must be selected',
  firstNameMinLength: 'First name must be at least 2 characters long',
  firstNameMaxLength: 'First name must not exceed 50 characters',
  lastNameMinLength: 'Last name must be at least 2 characters long',
  lastNameMaxLength: 'Last name must not exceed 50 characters',
  MolMinLength: 'MOL must be at least 2 characters long',
  MolMaxLength: 'MOL must not exceed 150 characters',
  countryMinLength: 'Country must be at least 2 characters long',
  countryMaxLength: 'Country must not exceed 60 characters',
  invalidPhoneNumber: 'Must be a valid phone number',
  cityMinLength: 'City must be at least 2 characters long',
  cityMaxLength: 'City must not exceed 50 characters',
  addressMinLength: 'Address must be at least 5 characters long',
  addressMaxLength: 'Address must not exceed 100 characters',
  companyMinLength: 'Company must be at least 3 characters long',
  companyMaxLength: 'Company must not exceed 50 characters',
  invalidTaxId: 'Must be a valid taxId',
  invalidVatNumber: 'Must be a valid vat number',
  invalidCardholderName: 'Cardholder\'s name must be between 2 and 26 characters and can only contains lettrs, -, \', and .',
  invalidCardNumber: 'Must be a valid card number',
  invalidCardDate: 'Must be a valid month/year',
  invalidCVV: 'Must be a valid CVV',
  nameMinLenght: 'Name must be at least 3 characters long',
  nameMaxLenght: 'Name must not exceed 50 characters',
  selectPoint: 'Please select a point on the map',
  descriptionMaxLength: 'Description must not exceed 250 characters',
  validTime: 'Please enter a valid time, for example: 9:00'
};

export const validationRules = {
  username: {
    required: errorMessages.required,
    minLength: {
      value: 3,
      message: errorMessages.usernameMinLenght,
    },
    maxLength: {
      value: 20,
      message: errorMessages.usernameMaxLenght,
    },
  },
  email: {
    required: errorMessages.required,
    pattern: {
      value: /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
      message: errorMessages.invalidEmail,
    },
    maxLength: {
      value: 50,
      message: errorMessages.emailMaxLenght,
    },
  },
  password: {
    required: errorMessages.required,
    minLength: {
      value: 6,
      message: errorMessages.passwordMinLenght,
    },
    maxLength: {
      value: 20,
      message: errorMessages.passwordMaxLenght,
    },
  },
  confirmPassword: {
    required: errorMessages.required,
    validate: (value: string, password: string) =>
      value === password || errorMessages.passwordDontMatch,
  },
  power: {
    required: errorMessages.required,
    valueAsNumber: true,
    min: {
      value: 0,
      message: errorMessages.powerMinLength,
    },
  },
  price: {
    required: errorMessages.required,
    valueAsNumber: true,
    min: {
      value: 0,
      message: errorMessages.priceMinLength,
    },
  },
  parkingPrice: {
    required: errorMessages.required,
    valueAsNumber: true,
    min: {
      value: 0,
      message: errorMessages.parkingPriceMinLength,
    },
  },
  initialFee: {
    required: errorMessages.required,
    valueAsNumber: true,
    min: {
      value: 0,
      message: errorMessages.initialFeeMinLength,
    },
  },
  type: {
    required: errorMessages.typeRequired,
  },
  object: {
    required: errorMessages.objectRequired,
  },
  model: {
    required: errorMessages.modelRequired,
  },
  firstName: {
    required: errorMessages.required,
    minLength: {
      value: 2,
      message: errorMessages.firstNameMinLength
    },
    maxLength: {
      value: 50,
      message: errorMessages.firstNameMaxLength
    }
  },
  lastName: {
    required: errorMessages.required,
    minLength: {
      value: 2,
      message: errorMessages.lastNameMinLength
    },
    maxLength: {
      value: 50,
      message: errorMessages.lastNameMaxLength
    }
  },
  MOL: {
    required: errorMessages.required,
    minLength: {
      value: 2,
      message: errorMessages.MolMinLength
    },
    maxLength: {
      value: 150,
      message: errorMessages.MolMaxLength
    }
  },
  country: {
    required: errorMessages.required,
    minLength: {
      value: 2,
      message: errorMessages.countryMinLength
    },
    maxLength: {
      value: 60,
      message: errorMessages.countryMaxLength
    }
  },
  phoneNumber: {
    required: errorMessages.required,
    valueAsNumber: true,
    validate: (isValid: boolean) => isValid || errorMessages.invalidPhoneNumber
  },
  city: {
    required: errorMessages.required,
    minLength: {
      value: 2,
      message: errorMessages.cityMinLength
    },
    maxLength: {
      value: 50,
      message: errorMessages.cityMaxLength
    }
  },
  address: {
    required: errorMessages.required,
    minLength: {
      value: 5,
      message: errorMessages.addressMinLength
    },
    maxLength: {
      value: 100,
      message: errorMessages.addressMaxLength
    }
  },
  company: {
    required: errorMessages.required,
    minLength: {
      value: 3,
      message: errorMessages.companyMinLength
    },
    maxLength: {
      value: 50,
      message: errorMessages.companyMaxLength
    }
  },
  taxId: {
    required: errorMessages.required,
  },
  vatNumber: {
    required: errorMessages.required
  },
  cardholderName: {
    required: errorMessages.required,
    pattern: {
      value: /^(?<! )[-a-zA-Z'. ]{2,26}$/,
      message: errorMessages.invalidCardholderName
    }
  },
  cardNumber: {
    required: errorMessages.required,
    pattern: {
      value: /^\b(?:\d[ -]*?){13,16}\b$/,
      message: errorMessages.invalidCardNumber
    }
  },
  cardDate: {
    required: errorMessages.required,
    pattern: {
      value: /^(0[1-9]|1[0-2])\/([0-9]{4}|[0-9]{2})$/,
      message: errorMessages.invalidCardDate
    }
  },
  cardCVV: {
    required: errorMessages.required,
    pattern: {
      value: /^[0-9]{3,4}$/,
      message: errorMessages.invalidCVV
    }
  },
  objectname: {
    required: errorMessages.required,
    minLength: {
      value: 3,
      message: errorMessages.nameMinLenght,
    },
    maxLength: {
      value: 50,
      message: errorMessages.nameMaxLenght,
    },
  },
  latitude: {
    required: errorMessages.selectPoint,
  },
  longitude: {
    required: errorMessages.selectPoint,
  },
  description: {
    maxLength: {
      value: 250,
      message: errorMessages.descriptionMaxLength
    }
  },
  hour: {
    required: errorMessages.required,
    pattern: {
      value: /^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/,
      message: errorMessages.validTime
    }
  },
};

