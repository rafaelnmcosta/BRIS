import React from 'react';

const PageWrapper = ({ children }) => {
  return (
    <div className="pt-16 px-24 bg-background-custom min-h-screen">
        {children}
    </div>

  );
};

export default PageWrapper;
